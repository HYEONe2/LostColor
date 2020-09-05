﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_sc : MonoBehaviour
{
    private ParticleSystem ps;

    GameObject Player;
    public bool m_bPlayerUse = false;
    //private Vector3 CameraDir;
    float fSpeed = 1.0f;
    [SerializeField] private GameObject Blood;


    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 2);

        Player = GameObject.Find("Player");
        //CameraDir = GameObject.Find("MainCamera").transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bPlayerUse)
        {
            Vector3 vPlayerTrans = Player.transform.position;
            Vector3 CameraDir = GameObject.Find("MainCamera").transform.forward;

            transform.position = new Vector3(vPlayerTrans.x + CameraDir.x *2f, vPlayerTrans.y + 1.4f, vPlayerTrans.z + CameraDir.z * 2f);
            transform.forward = CameraDir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_bPlayerUse)
        {
            if(other.gameObject.CompareTag("Boss"))
            {
                Vector3 OriginPos = gameObject.transform.position;
                Instantiate(Blood, OriginPos, Quaternion.identity);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Vector3 OriginPos = gameObject.transform.position;
                Instantiate(Blood, OriginPos, Quaternion.identity);
                //gameObject.SetActive(false);
            }
        }
    }
}
