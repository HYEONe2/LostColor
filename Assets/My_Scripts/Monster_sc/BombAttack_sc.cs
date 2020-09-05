﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack_sc : MonoBehaviour
{
    private GameObject Target;
    private Vector3 TargetPos;
    private float fBombtime = 0.0f;
    private bool bEffectOn = false;

    [SerializeField] private GameObject Effect;

    public bool m_bPlayerUse = false;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;
        Destroy(gameObject, 2.5f);

        if(m_bPlayerUse)
            dir = GameObject.Find("MainCamera").transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bPlayerUse)
        {
            gameObject.transform.position += dir * Time.deltaTime * 10f;
        }
        else
        {
            //float fDist = (TargetPos - gameObject.transform.position).magnitude;

            //Vector3 vDir = (TargetPos - gameObject.transform.position).normalized;
            //if (fDist > 1)
            //    gameObject.transform.position += vDir * Time.deltaTime * 11.0f;
            fBombtime += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.4f);
            //transform.Rotate(new Vector3(0, 0, 3));

            if (fBombtime > 2.0f && !bEffectOn)
            {
                Instantiate(Effect, transform.position, Quaternion.identity);
                bEffectOn = true;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (m_bPlayerUse)
        {
            if (other.gameObject.CompareTag("Boss"))
            {
                if (!bEffectOn)
                {
                    Instantiate(Effect, transform.position, Quaternion.identity);
                    bEffectOn = true;
                }
            }
        }
    }
}
