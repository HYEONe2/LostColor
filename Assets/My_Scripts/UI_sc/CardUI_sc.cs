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

        switch(eSkill)
        {
            case SKILL.SKILL_1:
                CanvasMgr_sc.m_CardList[0] = this;
                break;
            case SKILL.SKILL_2:
                CanvasMgr_sc.m_CardList[1] = this;
                break;
            case SKILL.SKILL_3:
                CanvasMgr_sc.m_CardList[2] = this;
                break;
        }

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
                        playerScript.SetSkill(0, Player_sc.SKILL.SKILL_WIND);
                        break;
                    case SKILL.SKILL_2:
                        playerScript.SetSkill(0, Player_sc.SKILL.SKILL_POISON);
                        break;
                    case SKILL.SKILL_3:
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
                        playerScript.SetSkill(1, Player_sc.SKILL.SKILL_ROCK);
                        break;
                    case SKILL.SKILL_2:
                        playerScript.SetSkill(1, Player_sc.SKILL.SKILL_CLOUD);
                        break;
                    case SKILL.SKILL_3:
                        int iSkill = 0;
                        do
                        {
                            iSkill = Random.Range(1, 3);

                        } while (iSkill == (int)playerScript.GetSkill(0));
                        playerScript.SetSkill(1, (Player_sc.SKILL)iSkill);
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
                        playerScript.SetSkill(2, Player_sc.SKILL.SKILL_BOMB);
                        break;
                    case SKILL.SKILL_2:
                        playerScript.SetSkill(2, Player_sc.SKILL.SKILL_SMOKE);
                        break;
                    case SKILL.SKILL_3:
                        int iSkill = 0;
                        do
                        {
                            iSkill = Random.Range(4, 5);
                            Debug.Log("ReSelect");
                        } while (iSkill == (int)playerScript.GetSkill(1));
                        Debug.Log(iSkill + "\t" + (int)playerScript.GetSkill(1));
                        playerScript.SetSkill(2, (Player_sc.SKILL)iSkill);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.loop = false;
        audio.clip = Resources.Load<AudioClip>("Sound/UI/CardSelect");
        audio.Play();
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
