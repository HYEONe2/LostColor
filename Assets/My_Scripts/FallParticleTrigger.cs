using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallParticleTrigger : MonoBehaviour
{
    public CameraManager CameraMgrScript;
    public MovingTree MovingTree;

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
            //MovingTree.Move_Tree(5f);
        }
    }
}
