using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CardUI_sc : MonoBehaviour, IPointerDownHandler
{
    private enum SKILL { SKILL_1, SKILL_2, SKILL_3, SKILL_END};
    [SerializeField] private SKILL eSkill;

    bool m_bTouch = false;
    float m_fRotate = 0;

    string m_strScene;
    int m_iScene;

    float m_fCheckTime = 0;

    Player_sc playerScript;

    // Start is called before the first frame update
    void Start()
    {
        m_strScene = SceneManager.GetActiveScene().name;

        if (m_strScene == "Stage_1")
            m_iScene = 0;
        else if (m_strScene == "Stage_2")
            m_iScene = 1;
        else if (m_strScene == "Stage_3")
            m_iScene = 2;

        CanvasMgr_sc.m_CardList.Add(this);

        playerScript = GameObject.Find("Player").GetComponent<Player_sc>();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bTouch)
        {
            if (playerScript.GetSkill(m_iScene) == Player_sc.SKILL.SKILL_END)
            {
                CheckSkill();
                CanvasMgr_sc.m_bIsClicked = true;
            }
            else
            {
                Debug.Log(m_fCheckTime);
                if (m_fCheckTime >= 2.0f)
                {
                    CanvasMgr_sc.SetCanvasReset();
                    m_fCheckTime = 0;
                }

                m_fCheckTime += Time.deltaTime;
            }
        }
    }

    void CheckSkill()
    {
        if (m_fRotate <= 180.0f)
        {
            m_fRotate += Time.deltaTime * 100.0f;
            this.transform.Rotate(Vector3.up * Time.deltaTime * 100.0f);

            if (eSkill == SKILL.SKILL_1)
                CanvasMgr_sc.m_bIsTurn[0] = true;
            else if (eSkill == SKILL.SKILL_2)
                CanvasMgr_sc.m_bIsTurn[1] = true;
            else if (eSkill == SKILL.SKILL_3)
                CanvasMgr_sc.m_bIsTurn[2] = true;
        }
        else
        {
            if (m_iScene == 0)
            {
                switch (eSkill)
                {
                    case SKILL.SKILL_1:
                        Debug.Log("1");
                        playerScript.SetSkill(0, Player_sc.SKILL.SKILL_WIND);
                        break;
                    case SKILL.SKILL_2:
                        Debug.Log("2");
                        playerScript.SetSkill(0, Player_sc.SKILL.SKILL_POISON);
                        break;
                    case SKILL.SKILL_3:
                        Debug.Log("3");
                        playerScript.SetSkill(0, Player_sc.SKILL.SKILL_NUT);
                        break;
                    default:
                        break;
                }
            }
            else if (m_iScene == 1)
            {
                switch (eSkill)
                {
                    case SKILL.SKILL_1:
                        break;
                    case SKILL.SKILL_2:
                        break;
                    case SKILL.SKILL_3:
                        break;
                    default:
                        break;
                }
            }
            else if (m_iScene == 2)
            {
                switch (eSkill)
                {
                    case SKILL.SKILL_1:
                        break;
                    case SKILL.SKILL_2:
                        break;
                    case SKILL.SKILL_3:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnTouch(Vector2 vecTouch)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void SetUnSelectedCard()
    {
        gameObject.transform.localScale *= 0.94f;
    }
}
