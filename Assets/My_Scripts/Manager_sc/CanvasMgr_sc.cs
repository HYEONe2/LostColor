using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMgr_sc : MonoBehaviour
{
    [SerializeField] private Monster_sc monsterScript;

    GameObject JoystickCanvas;
    GameObject SkillCanvas;
    GameObject CardCanvas;

    public static List<CardUI_sc> m_CardList = new List<CardUI_sc>();
    public static bool[] m_bIsTurn = new bool[3];

    public static bool m_bIsClicked = false;
    static bool m_bReset = false;
    bool m_bDestroy = false;

    static float m_fCheckTime = 0;
    static float m_fTurnStage = 0;

    // Start is called before the first frame update
    void Start()
    {
        JoystickCanvas = GameObject.Find("JoyStickCanvas");
        SkillCanvas = GameObject.Find("SkillCanvas");
        CardCanvas = GameObject.Find("CardCanvas");

        JoystickCanvas.SetActive(true);
        SkillCanvas.SetActive(true);
        CardCanvas.SetActive(false);

        for(int i=0;i<3; ++i)
            m_bIsTurn[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bReset)
        {
            if (CardCanvas.gameObject.activeSelf)
            {
                JoystickCanvas.SetActive(true);
                SkillCanvas.SetActive(true);
                CardCanvas.gameObject.SetActive(false);
            }

            m_fTurnStage += Time.deltaTime;
            if (m_fTurnStage > 2.0f)
            {
                m_fTurnStage = 0;
                m_bReset = false;
                LodingSceneManager_sc.LoadScene("MainStage");
            }
        }

        if (monsterScript.nextState == Monster_sc.CurrentState.dead)
        {
            if (!m_bDestroy && m_fCheckTime >= 4.0f)
            {
                JoystickCanvas.SetActive(false);
                SkillCanvas.SetActive(false);
                CardCanvas.gameObject.SetActive(true);

                m_bDestroy = true;
            }

            m_fCheckTime += Time.deltaTime;
        }

        if (m_bIsClicked && CardCanvas.gameObject.activeSelf)
        {
            CheckCard();
        }
    }

    void CheckCard()
    {
        for(int i=0;i<m_CardList.Count; ++i)
        {
            if (m_bIsTurn[i])
                continue;
            else
            {
                m_CardList[i].SetUnSelectedCard();
            }
        }
    }

    public static void SetCanvasReset()
    {
        m_bReset = true;
        m_bIsClicked = false;
        m_fCheckTime = 0;
    }
}
