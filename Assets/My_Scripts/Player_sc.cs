using UnityEngine;
using System.Collections.Generic;

public class Player_sc : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_turnSpeed = 180f;
    [SerializeField] private float m_jumpForce = 100f;

    private Animator m_animator;
    private Rigidbody m_rigidBody;
    private Transform tr;
    private Transform BossTrans;

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

    public GameObject Orb;
    public GameObject Shield;

    bool bOrbOnce = false;    // 행동따라 Orb 생성 제어
    bool bShieldOnce = false;   // 쉴드 생성 제어

    // 시간따라 Orb 생성 제어
    bool bOrbCreate = false;
    float fOrbCheckTime = 0;

    private Transform MainCameraPos;
    private ObjectMgr_sc ObjMgrScript;

    void Awake()
    {
        if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
        if (!tr) { tr = gameObject.GetComponent<Transform>(); }
        if (!BossTrans) { BossTrans = GameObject.Find("BossMonster").transform; }
        if (!MainCameraPos) { MainCameraPos = GameObject.Find("MainCamera").transform; }
        if (!ObjMgrScript) { ObjMgrScript = GameObject.Find("ObjectManager").GetComponent<ObjectMgr_sc>(); }
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
        if (Input.GetKeyDown(KeyCode.I))
        {
            CheckSkillCount();
        }

        if (bOrbCreate)
            fOrbCheckTime += Time.deltaTime;
        if (fOrbCheckTime > 10.0f)
        {
            bOrbCreate = false;
            fOrbCheckTime = 0;
        }

        //if(Input.GetKeyDown(KeyCode.O))
        //{

        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //}
    }

    void FixedUpdate()
    {
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
        float dist = (tr.position - BossTrans.position).magnitude;
        if (dist < 5.0f)
        {
            m_animator.SetBool("MainAtt", true);
        }
        else
        {
            m_animator.SetBool("LMainAtt", true);
        }
    }

    void SetAttackFalse()
    {
        m_animator.SetBool("MainAtt", false);

        // LMainAtt : MainAtt & OrbAtt 공유
        bOrbOnce = false;
        m_animator.SetBool("LMainAtt", false);
    }

    public void ShieldAttack()
    {
        if (CheckSkillCount() == 3)
            return;

        bShieldOnce = false;
        m_animator.SetBool("ShieldAtt", true);
    }

    public void SetShieldAttackFalse()
    {
        if (bShieldOnce)
            return;

        bShieldOnce = true;
        m_animator.SetBool("ShieldAtt", false);

        Vector3 OriginPos = tr.transform.position;
        Instantiate(Shield, new Vector3(OriginPos.x, OriginPos.y + 1.0f, OriginPos.z + 1.0f), Quaternion.identity);
        Instantiate(Shield, new Vector3(OriginPos.x + 1.0f, OriginPos.y + 1.0f, OriginPos.z - 0.5f), Quaternion.identity);
        Instantiate(Shield, new Vector3(OriginPos.x - 1.0f, OriginPos.y + 1.0f, OriginPos.z - 0.5f), Quaternion.identity);
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

        Vector3 OriginPos = gameObject.transform.position;
        Vector3 dir = GameObject.Find("MainCamera").transform.forward;
        Instantiate(Orb, new Vector3(OriginPos.x + dir.x, OriginPos.y + 1.5f, OriginPos.z + dir.z), Quaternion.identity);

        bOrbCreate = true;
    }

    int CheckSkillCount()
    {
        int iCnt = ObjMgrScript.CountOfObject(ObjectMgr_sc.OBJECT.OBJ_SHIELD);

        return iCnt;
    }
}
