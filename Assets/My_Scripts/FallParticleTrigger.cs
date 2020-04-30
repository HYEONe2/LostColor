using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallParticleTrigger : MonoBehaviour
{
    public CameraManager CameraMgrScript;
    public MovingTree RightTree;
    public MovingTree LeftTree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("FallParticleTrigger & Player");

            entry Entry = GameObject.Find("Entry").GetComponent<entry>();
            Entry.entry_on = true;

            CameraMgrScript.ShakingCameraOn();
            RightTree.entry_on = true;
            LeftTree.entry_on = true;
        }
    }
}
