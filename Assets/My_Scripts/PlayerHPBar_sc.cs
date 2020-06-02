using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar_sc : MonoBehaviour
{
    public Image hpBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Update_PlayerHP();
    }

    void Update_PlayerHP()
    {
        int hp = Player_sc.m_Hp;

        hpBar.fillAmount = hp / 10f;
    }
}
