﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Random;


public class Monster_sc : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private Transform tr;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform weapon;
    [SerializeField] private GameObject posion;
    [SerializeField] private GameObject wind;
    [SerializeField] private GameObject nut;
    [SerializeField] private NavMeshAgent nav;


    public enum CurrentState { idle, walk, attck_wind, attack_poison, attack_nut, hit, dead };
    public CurrentState curState = CurrentState.dead;
    public CurrentState nextState = CurrentState.idle;


    public float traceDist = 10.0f;
    public float attackDist = 3.2f;
    private bool isDead = false;
    public static int iHp = 10;
    //private bool bIsAttack = false;

    public void Initialize(GameObject character)
    {
        m_animator = character.GetComponent<Animator>();
        m_rigidBody = character.GetComponent<Rigidbody>();
    }

    void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }
    }
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");

        StartCoroutine(this.CheackState());
        StartCoroutine(this.CheckStateForAction());
    }

    void Update()
    {

    }

    IEnumerator CheackState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(0.3f);

            //if(curState != nextState)
            //    curState = nextState; //현재 스테이트키 바꿔줌

            float dist = Vector3.Distance(target.transform.position, tr.position);
            

            if (dist <= attackDist&& (curState != nextState))
            {
                curState = nextState;
                nextState = (CurrentState)Random.Range(2, 5);
                switch (nextState)
                {
                    case CurrentState.attack_poison:
                        if (m_animator.GetBool("PoisonAttack"))
                        {
                            Instantiate(posion, weapon.transform.position, transform.rotation); // 타이밍 조절해줘야함                           
                        }
                        break;
                    case CurrentState.attck_wind:
                        if (m_animator.GetBool("WindAttack"))
                        {
                            Instantiate(wind, weapon.transform.position, transform.rotation); // 타이밍 조절해줘야함                           
                        }
                        break;
                    case CurrentState.attack_nut:
                        break;
                    default:
                        break;
                }

            }

            else if (dist >= traceDist || dist > attackDist)
            {
                nextState = CurrentState.walk;
            }

            else
            {
                nextState = CurrentState.idle;
            }    
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (nextState)
            {
                case CurrentState.idle:
                    nav.Stop();
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", false);
                    m_animator.SetBool("Hit", false);

                    break;
                case CurrentState.walk:
                    nav.destination = target.transform.position;
                    nav.Resume();
                    m_animator.SetBool("Walk", true);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", false);
                    m_animator.SetBool("Hit", false);

                    break;
                case CurrentState.attck_wind:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", false);
                    m_animator.SetBool("WindAttack", true);

                    break;
                case CurrentState.attack_poison:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("NutAttack", false);
                    m_animator.SetBool("PoisonAttack", true);

                    break;
                case CurrentState.attack_nut:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", true);

                    break;
            }

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
           // Debug.Log("충돌!!!");
            m_animator.SetBool("Walk", false);
            m_animator.SetBool("WindAttack", false);
            m_animator.SetBool("PoisonAttack", false);
            m_animator.SetBool("NutAttack", false);
            m_animator.SetBool("NutAttack", false);
            m_animator.SetBool("Hit", true);
            iHp--; Debug.Log(iHp);
            if(iHp<0)
            {
                m_animator.SetBool("Death", true);
            }
        }
    }
}
