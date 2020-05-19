using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_sc : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private Transform tr;
    [SerializeField] private GameObject target;
    [SerializeField] private NavMeshAgent nav;

    public enum CurrentState {idle, walk, attck_wind, attack_poison, attack_nut, hit, dead };
    public CurrentState curState = CurrentState.idle;

    public float traceDist = 10.0f;
    public float attackDist = 3.2f;
    private bool isDead = false;

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
        while(!isDead)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(target.transform.position, tr.position);

            if (dist <= attackDist)
            {
                switch (curState)
                {
                    case CurrentState.attack_poison:
                        curState = CurrentState.attck_wind;
                        break;
                    case CurrentState.attck_wind:
                        curState = CurrentState.attack_nut;
                        break;
                    case CurrentState.attack_nut:
                        curState = CurrentState.attack_poison;
                        break;
                    default:
                        curState = CurrentState.attack_poison;
                        break;
                }

            }
            else if(dist >= traceDist|| dist > attackDist)
            {
                curState = CurrentState.walk;
            }
            else
            {
                curState = CurrentState.idle;
            }
        }
    }

    IEnumerator CheckStateForAction()
    {
        while (!isDead)
        {
            switch (curState)
            {
                case CurrentState.idle:
                    nav.Stop();
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", false);
                    break;
                case CurrentState.walk:
                    nav.destination = target.transform.position;
                    nav.Resume();
                    m_animator.SetBool("Walk", true);
                    m_animator.SetBool("WindAttack", false);
                    m_animator.SetBool("PoisonAttack", false);
                    m_animator.SetBool("NutAttack", false);
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

}
