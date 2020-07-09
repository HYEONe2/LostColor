﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPaticleTrigger_stage3 : MonoBehaviour
{
    public CameraManager CameraMgrScript;
    public GameObject RightTree;
    public GameObject LeftTree;
    public GameObject Entry;

    private AudioSource EntrySound;

    private bool isUsed = false;
    private bool isPlay = false;

    private entry Entryclass;
    private MovingTree Rtreeclass;
    private MovingTree Ltreeclass;

    // Start is called before the first frame update
    void Start()
    {
        Entryclass = Entry.GetComponent<entry>();
        EntrySound = GetComponent<AudioSource>();
        Rtreeclass = RightTree.GetComponent<MovingTree>();
        Ltreeclass = LeftTree.GetComponent<MovingTree>();
        EntrySound.Stop();
        EntrySound.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsed && !Entryclass.entry_on && !isPlay)
        {
            EntrySound.Stop();
            isPlay = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isUsed && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_sc>().SetTriggerStop(true);

            EntrySound.Play();
            Entryclass.entry_on = true;

            CameraMgrScript.ShakingCamera3On();

            Rtreeclass.entry_on2 = true;
            Ltreeclass.entry_on2 = true;

            isUsed = true;
        }
    }
}