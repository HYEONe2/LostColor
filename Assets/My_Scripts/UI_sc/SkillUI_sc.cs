using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillUI_sc : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public enum SKILL { SKILL_MAIN, SKILL_ORB, SKILL_SHIELD, SKILL_FIRST, SKILL_SECOND, SKILL_THIRD, SKILL_END};
    public SKILL eSKill;

    Player_sc playerScript;
    static List<GameObject> shieldGaugeList = new List<GameObject>();

    RectTransform[] m_rectSkill = new RectTransform[6];
    float[] m_fRadius = new float[6];

    bool m_bTouch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        switch(eSKill)
        {
            case SKILL.SKILL_MAIN:
                m_rectSkill[0] = transform.Find("../MainAttack").GetComponent<RectTransform>();
                m_fRadius[0] = m_rectSkill[0].rect.width * 0.5f;
                break;
            case SKILL.SKILL_ORB:
                m_rectSkill[1] = transform.Find("../OrbAttack").GetComponent<RectTransform>();
                m_fRadius[1] = m_rectSkill[1].rect.width * 0.5f;
                break;
            case SKILL.SKILL_SHIELD:
                m_rectSkill[2] = transform.Find("../ShieldAttack").GetComponent<RectTransform>();
                m_fRadius[2] = m_rectSkill[2].rect.width * 0.5f;
                break;
            case SKILL.SKILL_FIRST:
                m_rectSkill[3] = transform.Find("../FirstAttack").GetComponent<RectTransform>();
                m_fRadius[3] = m_rectSkill[3].rect.width * 0.5f;
                break;
            case SKILL.SKILL_SECOND:
                m_rectSkill[4] = transform.Find("../SecondAttack").GetComponent<RectTransform>();
                m_fRadius[4] = m_rectSkill[4].rect.width * 0.5f;
                break;
            case SKILL.SKILL_THIRD:
                m_rectSkill[5] = transform.Find("../ThirdAttack").GetComponent<RectTransform>();
                m_fRadius[5] = m_rectSkill[5].rect.width * 0.5f;
                break;
        }

        if (shieldGaugeList.Count == 0)
        {
            shieldGaugeList.Add(GameObject.Find("ShieldGauge1"));
            shieldGaugeList.Add(GameObject.Find("ShieldGauge2"));
            shieldGaugeList.Add(GameObject.Find("ShieldGauge3"));
        }

        playerScript = GameObject.Find("Player").GetComponent<Player_sc>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAlpha();

        if (m_bTouch)
        {
            switch (eSKill)
            {
                case SKILL.SKILL_MAIN:
                    playerScript.MainAttack();
                    break;
                case SKILL.SKILL_ORB:
                    playerScript.OrbAttack();
                    break;
                case SKILL.SKILL_SHIELD:
                    playerScript.ShieldAttack();
                    break;
                case SKILL.SKILL_FIRST:
                    playerScript.FirstAttack();
                    break;
                case SKILL.SKILL_SECOND:
                    playerScript.SecondAttack();
                    break;
                case SKILL.SKILL_THIRD:
                    playerScript.ThirdAttack();
                    break;
            }
        }
    }

    void OnTouch(Vector2 vecTouch)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_bTouch = false;
    }

    public static void TurnOffShieldGauge(int offCnt)
    {
        switch (offCnt)
        {
            case 1:
                shieldGaugeList[0].SetActive(false);
                break;
            case 2:
                shieldGaugeList[0].SetActive(false);
                shieldGaugeList[1].SetActive(false);
                break;
            case 3:
                shieldGaugeList[0].SetActive(false);
                shieldGaugeList[1].SetActive(false);
                shieldGaugeList[2].SetActive(false);
                break;
        }
    }

    public static void TurnOnShieldGauge()
    {
        if (!shieldGaugeList[0].activeSelf)
            shieldGaugeList[0].SetActive(true);
        else if (!shieldGaugeList[1].activeSelf)
            shieldGaugeList[1].SetActive(true);
        else if (!shieldGaugeList[2].activeSelf)
            shieldGaugeList[2].SetActive(true);
    }

    public static bool AllShieldOn()
    {
        if (shieldGaugeList[2] == null)
        {
            shieldGaugeList[0]= (GameObject.Find("ShieldGauge1"));
            shieldGaugeList[1] = (GameObject.Find("ShieldGauge2"));
            shieldGaugeList[2] = (GameObject.Find("ShieldGauge3"));
        }

        if (shieldGaugeList[2].activeSelf)
            return true;

        return false;
    }

    public static int GetShieldGaugeCnt()
    {
        int Cnt = 0;
        for (int i = 0; i < 3; ++i)
        {
            if (shieldGaugeList[i].activeSelf)
                ++Cnt;
        }

        return Cnt;
    }

    void ChangeAlpha()
    {
        float alpha = 1f;
        switch (eSKill)
        {
            case SKILL.SKILL_FIRST:
                if(playerScript.GetSkillOnce(0))
                    alpha = playerScript.GetCoolTime(0) / playerScript.GetCoolCheckTime(0);
                break;
            case SKILL.SKILL_SECOND:
                if (playerScript.GetSkillOnce(1))
                    alpha = playerScript.GetCoolTime(1) / playerScript.GetCoolCheckTime(1);
                break;
            case SKILL.SKILL_THIRD:
                if (playerScript.GetSkillOnce(2))
                    alpha = playerScript.GetCoolTime(2) / playerScript.GetCoolCheckTime(2);
                break;
        }

        Color originColor = gameObject.GetComponent<Image>().color;
        gameObject.GetComponent<Image>().color = new Vector4(originColor.r, originColor.g, originColor.b, alpha);
    }

    public void SetTexture(int iSkillNum)
    {
        Sprite SkillImage = null;
        switch (iSkillNum)
        {
            case 0:
                SkillImage = Resources.Load<Sprite>("Texture/UI/wind");
                break;
            case 1:
                SkillImage = Resources.Load<Sprite>("Texture/UI/poison");
                break;
            case 2:
                SkillImage = Resources.Load<Sprite>("Texture/UI/stones");
                break;
            case 3:
                SkillImage = Resources.Load<Sprite>("Texture/UI/rock");
                break;
            case 4:
                SkillImage = Resources.Load<Sprite>("Texture/UI/cloud");
                break;
        }
        gameObject.GetComponent<Image>().sprite = SkillImage;
    }
}
