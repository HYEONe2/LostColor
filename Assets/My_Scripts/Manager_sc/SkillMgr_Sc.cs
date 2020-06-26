﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMgr_Sc : MonoBehaviour
{
    public static SkillMgr_Sc Instance;

    private GameObject Player;
    private Transform PlayerTrans;
    private Vector3 PlayerPos;

    [SerializeField] private GameObject Orb;
    [SerializeField] private GameObject Shield;
    [SerializeField] private GameObject Wind;
    [SerializeField] private GameObject Poison;
    [SerializeField] private GameObject Nut;

    private float CheckShieldTime = 0;
    private bool ShieldOn = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerTrans = Player.transform;
        PlayerPos = PlayerTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        Shield_Update();
    }

    public void CreateOrb()
    {
        PlayerPos = PlayerTrans.position;

        Vector3 dir = GameObject.Find("MainCamera").transform.forward;
        Instantiate(Orb, new Vector3(PlayerPos.x + dir.x, PlayerPos.y + 1.5f, PlayerPos.z + dir.z), Quaternion.identity);
    }

    public void CreateShield()
    {
        int ShieldGaugeCnt = SkillUI_sc.GetShieldGaugeCnt();
        int ShieldCnt = ObjectMgr_sc.Instance.CountOfObject(ObjectMgr_sc.OBJECT.OBJ_SHIELD);
        int ShieldMake = 0;

        // 플레이어에 붙어있는 쉴드
        if(ShieldCnt == 0)
        {
            // UI 쉴드
            if (ShieldGaugeCnt == 3)
                ShieldMake = 3;
            else if (ShieldGaugeCnt == 2)
                ShieldMake = 2;
            else if (ShieldGaugeCnt == 1)
                ShieldMake = 1;
        }
        else if(ShieldCnt == 1)
        {
            if (ShieldGaugeCnt == 3)
                ShieldMake = 2;
            else if (ShieldGaugeCnt == 2)
                ShieldMake = 2;
            else if (ShieldGaugeCnt == 1)
                ShieldMake = 1;
        }
        else if (ShieldCnt == 2)
        {
            if (ShieldGaugeCnt == 3)
                ShieldMake = 1;
            else if (ShieldGaugeCnt == 2)
                ShieldMake = 1;
            else if (ShieldGaugeCnt == 1)
                ShieldMake = 1;
        }

        GameObject ShieldObj = null;
        PlayerPos = PlayerTrans.position;
        if (ShieldMake == 3)
        {
            Player_sc.m_ShieldGaugeList.Add(Instantiate(Shield, new Vector3(PlayerPos.x, PlayerPos.y + 1.0f, PlayerPos.z + 1.0f), Quaternion.identity));
            Player_sc.m_ShieldGaugeList.Add(Instantiate(Shield, new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity));
            Player_sc.m_ShieldGaugeList.Add(Instantiate(Shield, new Vector3(PlayerPos.x - 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity));

            SkillUI_sc.TurnOffShieldGauge(3);
        }
        else if(ShieldMake == 2)
        {
            if(ShieldCnt == 0)
            {
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x, PlayerPos.y + 1.0f, PlayerPos.z + 1.0f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[0] = ShieldObj;
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[1] = ShieldObj;
            }
            else if(ShieldCnt == 1)
            {
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[1] = ShieldObj;
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x - 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[2] = ShieldObj;
            }

            SkillUI_sc.TurnOffShieldGauge(2);
        }
        else if(ShieldMake == 1)
        {
            if (ShieldCnt == 0)
            {
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x, PlayerPos.y + 1.0f, PlayerPos.z + 1.0f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[0] = ShieldObj;
            }
            else if(ShieldCnt == 1)
            {
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x + 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[1] = ShieldObj;
            }
            else if(ShieldCnt == 2)
            {
                ShieldObj = Instantiate(Shield, new Vector3(PlayerPos.x - 1.0f, PlayerPos.y + 1.0f, PlayerPos.z - 0.5f), Quaternion.identity);
                Player_sc.m_ShieldGaugeList[2] = ShieldObj;
            }

            SkillUI_sc.TurnOffShieldGauge(1);
        }

        ShieldOn = true;
    }

    public void CreateWind()
    {
        GameObject WindOrb;
        PlayerPos = PlayerTrans.position;

        Vector3 dir = GameObject.Find("MainCamera").transform.forward;
        //WindOrb = Instantiate(Wind, new Vector3(PlayerPos.x + dir.x, PlayerPos.y + 1.5f, PlayerPos.z + dir.z), Quaternion.identity);
        WindOrb = Instantiate(Wind, Wind.transform.position, Wind.transform.rotation);                         

        WindOrb.GetComponent<Wind_sc>().m_bPlayerUse = true;
    }

    public void CreatePoison()
    {
        GameObject PoisonObj;
        PlayerPos = PlayerTrans.position;

        Vector3 dir = GameObject.Find("MainCamera").transform.forward;
        PoisonObj = Instantiate(Poison, new Vector3(PlayerPos.x + dir.x, PlayerPos.y + 1.5f, PlayerPos.z + dir.z), Quaternion.identity);

        PoisonObj.GetComponent<Monster_Bullet_sc>().m_bPlayerUse = true;
    }

    public void CreateNut()
    {
        //Instantiate();
    }

    private void Shield_Update()
    {
        if (ShieldOn)
            CheckShieldTime += Time.deltaTime;

        if (CheckShieldTime > 15.0f)
        {
            SkillUI_sc.TurnOnShieldGauge();
            CheckShieldTime = 0;
        }

        if (!ShieldOn)
            if (SkillUI_sc.AllShieldOn())
                ShieldOn = true;
    }
}