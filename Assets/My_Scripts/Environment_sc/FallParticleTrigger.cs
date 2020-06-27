using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallParticleTrigger : MonoBehaviour
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
            //Debug.Log("FallParticleTrigger & Player");
            EntrySound.Play();          
            Entryclass.entry_on = true;

            CameraMgrScript.ShakingCameraOn();

            Rtreeclass.entry_on = true;
            Ltreeclass.entry_on = true;

            isUsed = true;
        }
    }
}
