using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player_sc : MonoBehaviour
{
    // Player Component
    private Animator m_animator;
    private Rigidbody m_rigidBody;
    private Transform tr;
    
    // Needs
    private GameObject Boss;
    private bool IsBossAtt = false;
    private Transform MainCameraPos;
    private ObjectMgr_sc ObjMgrScript;

    // Player Info
    public static int m_Hp = 30;
    private float m_moveSpeed = 5f;
    private float m_turnSpeed = 180f;

    // Walk
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    // Jump
    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private float m_jumpForce = 6.5f;

    // Grounded
    private bool m_wasGrounded;
    private bool m_isGrounded;

    // MoveInput
    private float h = 0f;
    private float v = 0f;
    private float r = 0f;

    // Collision
    private List<Collider> m_collisions = new List<Collider>();
    
    // Shields
    public static GameObject[] m_ShieldGaugeList = new GameObject[3];

    // Skill Manage
    public enum SKILL { SKILL_WIND, SKILL_POISON, SKILL_NUT, SKILL_ROCK, SKILL_CLOUD, SKILL_END };
    public SKILL[] m_eSkill = new SKILL[3];
    
    bool bShieldOnce = false;   // 쉴드 생성 제어
    // 시간따라 Orb 생성 제어
    bool bOrbOnce = false;
    //bool bOrbCreate = false;
    //float fOrbCheckTime = 0;

    bool[] m_bSkillOnce = new bool[3];
    bool m_bAtt = false;

    //죽은 후 시간 지나고 씬 전환
    bool m_bDamaged = false;
    float m_fDamageTime = 0;
    float fDeadCheckTime = 0;

    enum STAGE { STAGE_MAIN, STAGE_1, STAGE_2, STAGE_3, STAGE_END};
    STAGE m_eStage = STAGE.STAGE_END;
    bool bPosInit = false;

    // Audio
    enum SOUND { SOUND_ATT, SOUND_HIT, SOUND_WIN, SOUND_DIE, SOUND_ORB, SOUND_SHIELD,
        SOUND_WIND, SOUND_POISON, SOUND_NUT, SOUND_ROCK, SOUND_CLOUD, SOUND_END };
    private AudioSource SoundAudio;
    private AudioSource EffectAudio;
    private List<AudioClip> Sound = new List<AudioClip>();

    public SKILL GetSkill(int iScene) { return m_eSkill[iScene];    }
    public void SetSkill(int iScene, SKILL eSkill) { m_eSkill[iScene] = eSkill; }
    public bool GetAtt() { return m_bAtt; }

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
        if (!tr) { tr = gameObject.GetComponent<Transform>(); }
        if (!MainCameraPos) { MainCameraPos = GameObject.Find("MainCamera").transform; }
        if (!ObjMgrScript) { ObjMgrScript = GameObject.Find("ObjectManager").GetComponent<ObjectMgr_sc>(); }

        for (int i = 0; i < 3; ++i)
        {
            m_eSkill[i] = SKILL.SKILL_END;
            m_bSkillOnce[i] = false;
        }

        this.SoundAudio = gameObject.AddComponent<AudioSource>();
        SoundAudio.loop = false;
        this.EffectAudio = gameObject.AddComponent<AudioSource>();
        EffectAudio.loop = false;

        Sound.Add(Resources.Load<AudioClip>("Sound/Player/PlayerAtt"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/PlayerHit"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/PlayerWin"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/PlayerDie"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/OrbSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/ShieldSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/WindSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/PoisonSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/NutsSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/RockSound"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Player/ThunderSound"));
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
            m_eStage = STAGE.STAGE_1;
        else if (SceneManager.GetActiveScene().name == "Stage_2")
            m_eStage = STAGE.STAGE_2;
        else if (SceneManager.GetActiveScene().name == "MainStage")
            m_eStage = STAGE.STAGE_MAIN;
        else
            m_eStage = STAGE.STAGE_END;

        SceneControl();

        // 죽음 처리
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

        // 피격 처리
        if(m_bDamaged)
            m_fDamageTime += Time.deltaTime;

        if(m_fDamageTime > 1f)
        {
            m_bDamaged = false;
            m_fDamageTime = 0;
        }

        //// Orb 공격 쿨타임
        //if (bOrbCreate)
        //    fOrbCheckTime += Time.deltaTime;

        //if (fOrbCheckTime > 0f)
        //{
        //    bOrbCreate = false;
        //    fOrbCheckTime = 0;
        //}
    }

    void FixedUpdate()
    {
        if (m_Hp <= 0 
            || m_animator.GetBool("Die") || m_animator.GetBool("Damaged") || m_animator.GetBool("Win"))
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
        if (m_animator.GetBool("Damaged"))
            return;

        if (!Boss)
        {
            m_animator.SetBool("LMainAtt", true);

            if (!m_bAtt)
                SoundPlay(SOUND.SOUND_ATT);
            m_bAtt = true;

            return;
        }

        if (!tr)
            Initialize();

        float dist = (tr.position - Boss.transform.position).magnitude;
        if (dist < 5.0f)
            m_animator.SetBool("MainAtt", true);
        else
            m_animator.SetBool("LMainAtt", true);

        if (!m_bAtt)
            SoundPlay(SOUND.SOUND_ATT);
        m_bAtt = true;
    }

    void SetAttackFalse()
    {
        // LMainAtt : MainAtt & OrbAtt 공유
        m_animator.SetBool("MainAtt", false);
        m_animator.SetBool("LMainAtt", false);
        m_animator.SetBool("Damaged", false);

        m_bAtt = false;
        bOrbOnce = false;
        m_bDamaged = false;
        for (int i = 0; i < 3; ++i)
            m_bSkillOnce[i] = false;

        EffectAudio.Stop();
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

        EffectSoundPlay(SOUND.SOUND_SHIELD);
        SkillMgr_Sc.Instance.CreateShield();

        bShieldOnce = true;
        m_animator.SetBool("ShieldAtt", false);
    }

    public void OrbAttack()
    {
        //if (bOrbCreate)
        //return;
        if (bOrbOnce)
            return;

        m_bAtt = true;
        bOrbOnce = true;
        m_animator.SetBool("LMainAtt", true);
    }

    public void CreateOrb()
    {
        if (!bOrbOnce /*|| bOrbCreate*/)
            return;

        EffectSoundPlay(SOUND.SOUND_ORB);
        SkillMgr_Sc.Instance.CreateOrb();
        //bOrbCreate = true;
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
                    EffectSoundPlay(SOUND.SOUND_WIND);
                    SkillMgr_Sc.Instance.CreateWind();
                }
                break;
            case SKILL.SKILL_POISON:
                {
                    m_animator.SetBool("LMainAtt", true);
                    EffectSoundPlay(SOUND.SOUND_POISON);
                    SkillMgr_Sc.Instance.CreatePoison();
                }
                break;
            case SKILL.SKILL_NUT:
                {
                    m_animator.SetBool("LMainAtt", true);
                    EffectSoundPlay(SOUND.SOUND_NUT);
                    SkillMgr_Sc.Instance.CreateNut();
                }
                break;
        }

        m_bAtt = true;
        m_bSkillOnce[0] = true;
    }

    public void SecondAttack()
    {
        if (m_bSkillOnce[1])
            return;

        switch (m_eSkill[1])
        {
            case SKILL.SKILL_ROCK:
                {
                    m_animator.SetBool("LMainAtt", true);
                    EffectSoundPlay(SOUND.SOUND_ROCK);
                    SkillMgr_Sc.Instance.CreateRock();
                }
                break;
            case SKILL.SKILL_CLOUD:
                {
                    m_animator.SetBool("LMainAtt", true);
                    EffectSoundPlay(SOUND.SOUND_CLOUD);
                    SkillMgr_Sc.Instance.CreateCloud();
                }
                break;
        }

        m_bAtt = true;
        m_bSkillOnce[1] = true;
    }

    public void ThirdAttack()
    {
        Debug.Log("3");
    }

    public void SetVictory()
    {
        SoundPlay(SOUND.SOUND_WIN);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss")
            || other.gameObject.CompareTag("MonsterWeapon"))
        {
            if (!IsBossAtt)
                return;

            int ShieldCnt = ObjectMgr_sc.Instance.CountOfObject(ObjectMgr_sc.OBJECT.OBJ_SHIELD);
            if (ShieldCnt != 0 && !m_bDamaged)
            {
                Destroy(m_ShieldGaugeList[--ShieldCnt]);
                return;
            }

            if (!m_bDamaged)
                SoundPlay(SOUND.SOUND_HIT);
            m_bDamaged = true;
            m_animator.SetBool("Damaged", true);

            m_Hp -= 1;
            if(m_Hp == 0)
                SoundPlay(SOUND.SOUND_DIE);
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
            SetAttackFalse();
            m_animator.SetBool("Win", false);
            m_animator.SetBool("Die", false);
            bPosInit = false;
        }
    }

    private void SoundPlay(SOUND eSound)
    {
        SoundAudio.clip = Sound[(int)eSound];
        SoundAudio.Play();
    }

    private void EffectSoundPlay(SOUND eSound)
    {
        EffectAudio.clip = Sound[(int)eSound];
        EffectAudio.Play();
    }

    private void SceneControl()
    {
        switch(m_eStage)
        {
            case STAGE.STAGE_MAIN:
                Stage_Main_PosInit();
                break;
            case STAGE.STAGE_1:
                Stage_1_PosInit();
                Boss = GameObject.FindWithTag("Boss");
                IsBossAtt = Boss.GetComponent<Monster_sc>().GetIsAttack();
                if (Boss.GetComponent<Monster_sc>().nextState == Monster_sc.CurrentState.dead)
                {
                    m_animator.SetBool("Win", true);
                    return;
                }
                break;
            case STAGE.STAGE_2:
                Stage_2_PosInit();
                Boss = GameObject.FindWithTag("Boss");
                IsBossAtt = Boss.GetComponent<Stage2Monster_sc>().GetIsAttack();
                if (Boss.GetComponent<Stage2Monster_sc>().nextState == Stage2Monster_sc.CurrentState.dead)
                {
                    m_animator.SetBool("Win", true);
                    return;
                }
                break;
            case STAGE.STAGE_3:
                break;
        }
    }
}
