using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillUI_sc : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    enum SKILL { SKILL_MAIN, SKILL_ORB, SKILL_SHIELD, SKILL_END};
    private SKILL eSKill;
    public int skillNum;

    public Player_sc playerScript;

    RectTransform m_rectMainSkill;
    float m_fRadius;

    RectTransform m_rectOrbSkill;
    float m_fOrbRadius;

    RectTransform m_rectShieldSkill;
    float m_fShieldRadius;

    bool m_bTouch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        switch(skillNum)
        {
            case 0:
                eSKill = SKILL.SKILL_MAIN;
                m_rectMainSkill = transform.Find("../MainAttack").GetComponent<RectTransform>();
                m_fRadius = m_rectMainSkill.rect.width * 0.5f;
                break;
            case 1:
                eSKill = SKILL.SKILL_ORB;
                m_rectOrbSkill = transform.Find("../OrbAttack").GetComponent<RectTransform>();
                m_fOrbRadius = m_rectOrbSkill.rect.width * 0.5f;
                break;
            case 2:
                eSKill = SKILL.SKILL_SHIELD;
                m_rectShieldSkill = transform.Find("../ShieldAttack").GetComponent<RectTransform>();
                m_fShieldRadius = m_rectShieldSkill.rect.width * 0.5f;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bTouch)
        {
            switch(eSKill)
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
}
