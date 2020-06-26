using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player_sc : MonoBehaviour
{
    private float m_moveSpeed = 5f;
    private float m_turnSpeed = 180f;
    [SerializeField] private float m_jumpForce = 100f;

    private Animator m_animator;
    private Rigidbody m_rigidBody;
    private Transform tr;
    private Transform BossTrans;
    public static int m_Hp = 100;

    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;

    private bool m_wasGrounded;
    private bool m_isGrounded;

    private float h = 0f;
    private float v = 0f;
    private float r = 0f;

    private List<Collider> m_collisions = new List<Collider>();
    public static List<GameObject> m_ShieldGaugeList = new List<GameObject>();

    //bool bOrbOnce = false;    // 행동따라 Orb 생성 제어
    bool bShieldOnce = false;   // 쉴드 생성 제어

    // 시간따라 Orb 생성 제어
    bool bOrbCreate = false;
    bool bOrbOnce = false;
    float fOrbCheckTime = 0;
    bool[] m_bSkillOnce = new bool[3];

    //죽은 후 시간 지나고 씬 전환
    float fDeadCheckTime = 0;

    bool bPosInit = false;

    private Transform MainCameraPos;
    private ObjectMgr_sc ObjMgrScript;

    public enum SKILL { SKILL_WIND, SKILL_POISON, SKILL_NUT, SKILL_END };
    public SKILL[] m_eSkill = new SKILL[3];

    public SKILL GetSkill(int iScene)
    {
        return m_eSkill[iScene];
    }

    public void SetSkill(int iScene, SKILL eSkill)
    {
        m_eSkill[iScene] = eSkill;
    }

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
        if (!tr) { tr = gameObject.GetComponent<Transform>(); }
        if (!BossTrans) { BossTrans = GameObject.Find("BossMonster").transform; }
        if (!MainCameraPos) { MainCameraPos = GameObject.Find("MainCamera").transform; }
        if (!ObjMgrScript) { ObjMgrScript = GameObject.Find("ObjectManager").GetComponent<ObjectMgr_sc>(); }

        for (int i = 0; i < 3; ++i)
        {
            m_eSkill[i] = SKILL.SKILL_END;
            m_bSkillOnce[i] = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Stage_1")
            Stage_1_PosInit();
        if (SceneManager.GetActiveScene().name == "Stage_2")
            Stage_2_PosInit();
        if (SceneManager.GetActiveScene().name == "MainStage")
            Stage_Main_PosInit();

        if (m_Hp <= 0)
        {
            m_animator.SetBool("Die", true);
            fDeadCheckTime += Time.deltaTime;
        }

        if (fDeadCheckTime > 3.0f)
        {
            fDeadCheckTime = 0.0f;
            m_Hp = 10;
            LodingSceneManager_sc.LoadScene("MainStage");
            return;
        }

        if (bOrbCreate)
            fOrbCheckTime += Time.deltaTime;

        if (fOrbCheckTime > 10.0f)
        {
            bOrbCreate = false;
            fOrbCheckTime = 0;
        }
    }

    void FixedUpdate()
    {
        if (m_Hp <= 0 || m_animator.GetBool("Die") || m_animator.GetBool("Damaged"))
            return;

        m_animator.SetBool("Grounded", m_isGrounded);

        DirectUpdate();

        m_wasGrounded = m_isGrounded;
    }

    private void DirectUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        bool run = Input.GetKey(KeyCode.LeftShift);
        if (v < 0)
        {
            if (run) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        }
        else if (run)
        {
            v *= m_walkScale;
        }

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        tr.Translate(moveDir.normalized * m_moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * m_turnSpeed * Time.deltaTime * h);

        m_animator.SetFloat("MoveSpeed", moveDir.magnitude);

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && Input.GetKey(KeyCode.Space))
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    public void MainAttack()
    {
        if (!tr)
            Initialize();

        float dist = (tr.position - BossTrans.position).magnitude;
        if (dist < 5.0f)
            m_animator.SetBool("MainAtt", true);
        else
            m_animator.SetBool("LMainAtt", true);
    }

    void SetAttackFalse()
    {
        m_animator.SetBool("MainAtt", false);

        // LMainAtt : MainAtt & OrbAtt 공유
        m_animator.SetBool("LMainAtt", false);
        m_animator.SetBool("Damaged", false);

        bOrbOnce = false;
        for (int i = 0; i < 3; ++i)
            m_bSkillOnce[i] = false;
    }

    public void ShieldAttack()
    {
        int skillCnt = ObjectMgr_sc.Instance.CountOfObject(ObjectMgr_sc.OBJECT.OBJ_SHIELD);
        int ShieldGaugeCnt = SkillUI_sc.GetShieldGaugeCnt();

        if (skillCnt == 3 || ShieldGaugeCnt == 0)
            return;

        bShieldOnce = false;
        m_animator.SetBool("ShieldAtt", true);
    }

    public void SetShieldAttackFalse()
    {
        if (bShieldOnce)
            return;

        SkillMgr_Sc.Instance.CreateShield();

        bShieldOnce = true;
        m_animator.SetBool("ShieldAtt", false);
    }

    public void OrbAttack()
    {
        if (bOrbCreate)
            return;

        bOrbOnce = true;
        m_animator.SetBool("LMainAtt", true);
    }

    public void CreateOrb()
    {
        if (!bOrbOnce || bOrbCreate)
            return;

        SkillMgr_Sc.Instance.CreateOrb();

        bOrbCreate = true;
    }

    public void FirstAttack()
    {
        if (m_bSkillOnce[0])
            return;

        switch (m_eSkill[0])
        {
            case SKILL.SKILL_WIND:
                {
                    m_animator.SetBool("LMainAtt", true);
                    SkillMgr_Sc.Instance.CreateWind();
                }
                break;
            case SKILL.SKILL_POISON:
                {
                    m_animator.SetBool("LMainAtt", true);
                    SkillMgr_Sc.Instance.CreatePoison();
                }
                break;
            case SKILL.SKILL_NUT:
                {
                    m_animator.SetBool("LMainAtt", true);
                    SkillMgr_Sc.Instance.CreateNut();
                }
                break;
        }

        m_bSkillOnce[0] = true;
    }

    public void SecondAttack()
    {
        Debug.Log("2");
    }

    public void ThirdAttack()
    {
        Debug.Log("3");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss")
            || other.gameObject.CompareTag("MonsterWeapon"))
        {
            int ShieldCnt = ObjectMgr_sc.Instance.CountOfObject(ObjectMgr_sc.OBJECT.OBJ_SHIELD);
            if (ShieldCnt != 0)
            {
                Destroy(m_ShieldGaugeList[--ShieldCnt]);
                return;
            }

            m_animator.SetBool("Damaged", true);
            m_Hp -= 1;
        }
    }

    private void Stage_1_PosInit()
    {
        if (!bPosInit)
        {
            transform.position = new Vector3(14.347f, 0.34f, -0.066f);
            bPosInit = true;
        }
    }
    private void Stage_2_PosInit()
    {
        if (!bPosInit)
        {
            transform.position = new Vector3(9.84f, 0.34f, -13.97f);
            bPosInit = true;
        }
    }
    private void Stage_Main_PosInit()
    {
        if (bPosInit)
        {
            transform.position = new Vector3(-19.0f, 0.0f, 20.0f);
            //Debug.Log("die :" + m_animator.GetBool("Die"));
            m_animator.SetBool("Die", false);
            bPosInit = false;
        }
    }
}
