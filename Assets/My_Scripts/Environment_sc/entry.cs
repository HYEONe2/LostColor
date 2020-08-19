﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entry : MonoBehaviour
{
    private float y_pos = 0.0f;
    private float deltatime;
    public float speed = 0.005f;
    public bool entry_on = false;
    private float originPos;

    // Start is called before the first frame update
    void Start()
    {
        entry_on = false;
        originPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (entry_on)
        {
            deltatime += Time.deltaTime;
            transform.Translate(0.0f, y_pos, 0.0f);    
            y_pos += speed * Time.deltaTime;
            if (transform.position.y - originPos >= 4.5f)
            {
                //Debug.Log("입구STOP"+ transform.position.y);
                GameObject.Find("Player").GetComponent<Player_sc>().SetTriggerStop(false);
                entry_on = false;
                //y_pos = 0.0f;
            }
        }
    }
}
