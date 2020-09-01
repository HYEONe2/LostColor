using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Monster3HPBar_sc : MonoBehaviour
{
    public Image hpbar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPbar();
    }

    public void PlayerHPbar()

    {

        float HP = Stage3Monster_sc.iHp; //캐릭터 hp를 받아옴

        hpbar.fillAmount = HP / 10f;

        //HpText.text = string.Format("HP {0}/100", HP);

    }
}
