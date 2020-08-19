using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Stage2Monster_sc : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private Transform tr;
    [SerializeField] private GameObject target;
    [SerializeField] private Transform RightHand;
    [SerializeField] private GameObject rock;
    [SerializeField] private GameObject cloud;
    //[SerializeField] private GameObject nut;
    [SerializeField] private NavMeshAgent nav;

    private AudioSource HitSound;

    public enum CurrentState { idle, walk, attck_1, attack_3, attack_2, hit, dead, end };
    public CurrentState curState = CurrentState.end;
    public CurrentState nextState = CurrentState.idle;
    private Stage_Manager triggerObj;


    public float traceDist = 20.0f;
    public float attackDist = 15.0f;
    private bool isDead = false;
    public static int iHp = 10;
    private RockAttack_sc Rockclass;
    private bool bIsAttack = false;
    private bool bIsHit = false;

    //// 시간따라 Orb 생성 제어
    bool bRockCreate = false;
    bool bCloudCreate = false;
    //bool bNutCreate = false;




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

        StartCoroutine(this.CheackState());
        StartCoroutine(this.CheckStateForAction());
        Rockclass = rock.GetComponent<RockAttack_sc>();
    }

    private void Update()
    {
        if (iHp <= 0) //몬스터 사망
        {
            nextState = CurrentState.dead;
            m_animator.SetBool("Die", true);
            isDead = true;
            triggerObj = (Stage_Manager)FindObjectOfType(typeof(Stage_Manager));
            triggerObj.stage1_open = false;
            triggerObj.stage2_open = false;
            triggerObj.stage3_open = true;
            curState = nextState;
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
            //Debug.Log(dist);
            if (dist <= 3.0f)
                nextState = CurrentState.attack_2;

            else if (dist <= attackDist && dist > 3.0f)
            {
                nextState = (CurrentState)Random.Range(2, 4);
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
                    m_animator.SetBool("Attack1", false);
                    m_animator.SetBool("Attack2", false);
                    m_animator.SetBool("Attack3", false);
                    m_animator.SetBool("Hit", false);

                    break;
                case CurrentState.walk:
                    nav.destination = target.transform.position;
                    nav.Resume();
                    m_animator.SetBool("Walk", true);
                    m_animator.SetBool("Attack1", false);
                    m_animator.SetBool("Attack2", false);
                    m_animator.SetBool("Attack3", false);
                    m_animator.SetBool("Hit", false);

                    break;
                case CurrentState.attck_1:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("Attack1", true);
                    m_animator.SetBool("Attack2", false);
                    m_animator.SetBool("Attack3", false);

                    break;
                case CurrentState.attack_2:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("Attack1", false);
                    m_animator.SetBool("Attack2", true);
                    m_animator.SetBool("Attack3", false);

                    break;
                case CurrentState.attack_3:
                    m_animator.SetBool("Walk", false);
                    m_animator.SetBool("Attack1", false);
                    m_animator.SetBool("Attack2", false);
                    m_animator.SetBool("Attack3", true);

                    break;
            }

            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon"))
        {
            if (!target.GetComponent<Player_sc>().GetAtt())
                return;
            // Debug.Log("충돌!!!");
            m_animator.SetBool("Walk", false);
            m_animator.SetBool("Attack1", false);
            m_animator.SetBool("Attack2", false);
            m_animator.SetBool("Attack3", false);
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
    public void EndAttack()
    {
        bIsAttack = false;
    }

    public void CreateRock()
    {
        if (bRockCreate)
            return;
        //Debug.Log("돌 공격");
        Instantiate(rock, new Vector3(gameObject.transform.position.x - 2.0f, gameObject.transform.position.y, gameObject.transform.position.z), gameObject.transform.rotation);
        RockAttack_sc.bIsMove = true;
        bRockCreate = true;
        bIsAttack = true;
    }
    public void ShootRock()
    {
        RockAttack_sc.bIsMove = false;
    }
    public void FalseRock()
    {
        if (bRockCreate)
            bRockCreate = false;
        bIsAttack = false;

    }

    public void CreateCloud()
    {
        if (bCloudCreate)
            return;
        //Debug.Log("구름 공격");
        Instantiate(cloud, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10.0f, gameObject.transform.position.z), gameObject.transform.rotation);
    }

    public void FalseCloud()
    {
        if (bCloudCreate)
            bCloudCreate = false;
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
