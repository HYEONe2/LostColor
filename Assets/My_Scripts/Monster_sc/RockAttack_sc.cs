﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockAttack_sc : MonoBehaviour
{
    GameObject Target;
    GameObject MonsterHand;
    Vector3 TargetPos;

    private AudioSource ShootSound;

    [SerializeField] private GameObject Blood;

    public static bool bIsMove = true;
    public bool m_bPlayerUse = false;
    private bool bIsPlay = false;

    private GameObject Monster;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        MonsterHand = GameObject.Find("Index_Proximal_R");
        TargetPos = Target.transform.position;
        Destroy(gameObject, 4);

        if (m_bPlayerUse)
        {
            Monster = GameObject.FindWithTag("Boss");
            if (Monster)
                TargetPos = Monster.transform.position;
        }
        else
        {
            ShootSound = GetComponent<AudioSource>();
            ShootSound.Stop();
            ShootSound.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bPlayerUse)
        {
            if (!Monster)
            {
                Vector3 dir = GameObject.Find("MainCamera").transform.forward;
                gameObject.transform.position += dir * Time.deltaTime * 10f;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.5f);
                transform.Rotate(new Vector3(0, 0, 3));
            }
        }
        else
        {
            if (!bIsMove)
            {
                if (!bIsPlay)
                {
                    ShootSound.Play();
                    bIsPlay = true;
                }
                transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.5f);
                transform.Rotate(new Vector3(0, 0, 3));
            }
            else
            {
                //Debug.Log("이동");
                transform.position = MonsterHand.transform.position;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 OriginPos = gameObject.transform.position;
            Instantiate(Blood, OriginPos, Quaternion.identity);
            gameObject.SetActive(false);
        }

    }
}
