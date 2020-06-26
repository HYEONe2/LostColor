using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster2HPBar_sc : MonoBehaviour
{
    public Image hpbar;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()

    {
        PlayerHPbar();
    }

    public void PlayerHPbar()

    {

        float HP = Stage2Monster_sc.iHp; //캐릭터 hp를 받아옴

        hpbar.fillAmount = HP / 10f;

        //HpText.text = string.Format("HP {0}/100", HP);

    }
}
