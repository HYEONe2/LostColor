using System.Collections;
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

    private AudioSource HitSound;

    public enum CurrentState { idle, walk, attck_wind, attack_poison, attack_nut, hit, dead, end };
    public CurrentState curState = CurrentState.end;
    public CurrentState nextState = CurrentState.idle;
    private Stage_Manager triggerObj;


    public float traceDist = 10.0f;
    public float attackDist = 3.2f;
    private bool isDead = false;
    public static int iHp = 10;
    private bool bIsAttack = false;
    private bool bIsHit = false;


    // 시간따라 Orb 생성 제어
    bool bPoisonCreate = false;
    bool bWindCreate = false;
    bool bNutCreate = false;


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

        HitSound = GetComponent<AudioSource>();
        HitSound.Stop();
        HitSound.playOnAwake = false;

        m_animator.SetBool("Death", false);

        StartCoroutine(this.CheackState());
        StartCoroutine(this.CheckStateForAction());
    }

    private void Update()
    {
        if (iHp <= 0) //몬스터 사망
        {

            m_animator.SetBool("Death", true);
            nextState = CurrentState.dead;
            isDead = true;
            triggerObj = (Stage_Manager)FindObjectOfType(typeof(Stage_Manager));
            triggerObj.stage1_open = false;
            triggerObj.stage2_open = true;
            triggerObj.stage3_open = false;
            curState = nextState;
            return;
        }
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
                nextState = (CurrentState)Random.Range(2, 5);             
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
            if (!target.GetComponent<Player_sc>().GetAtt())
                return;
            
            m_animator.SetBool("Walk", false);
            m_animator.SetBool("WindAttack", false);
            m_animator.SetBool("PoisonAttack", false);
            m_animator.SetBool("NutAttack", false);
            m_animator.SetBool("NutAttack", false);
            m_animator.SetBool("Hit", true);
            --iHp;
        }
    }

    public bool GetIsAttack()
    {
        return bIsAttack;
    }

    public void StartAttack()
    {
        bIsAttack = true;
    }

    public void CreatePoison()
    {
        if (bPoisonCreate)
            return;
        //Debug.Log("독 공격");                        
        Instantiate(posion, weapon.transform.position, transform.rotation);

        bPoisonCreate = true;
    }
    public void FalsePoison()
    {
        if (bPoisonCreate)
            bPoisonCreate = false;
        bIsAttack = false;
    }
    public void CreateWind()
    {
        if (bWindCreate)
            return;
        //Debug.Log("바람 공격");
        Instantiate(wind, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1.4f, gameObject.transform.position.z), gameObject.transform.rotation); // 타이밍 조절해줘야함   

        bWindCreate = true;
    }
    public void FalseWind()
    {
        if (bWindCreate)
            bWindCreate = false;
        bIsAttack = false;
    }
    public void CreateNut()
    {
        if (bNutCreate)
            return;
        //Debug.Log("밤송이 공격");
        Instantiate(nut, weapon.transform.position, transform.rotation); // 타이밍 조절해줘야함    

        bNutCreate = true;
    }
    public void FalseNut()
    {
        if (bNutCreate)
            bNutCreate = false;
        bIsAttack = false;
    }

    public void DamageAttack()
    {
        if (!bIsHit)
        {
            HitSound.Play();
            bIsHit = true;
        }
    }
    public void FalseHit()
    {
        HitSound.Stop();
        m_animator.SetBool("Hit", false);
        bIsHit = false;

    }
}
