using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallParticleTrigger : MonoBehaviour
{
    public CameraManager CameraMgrScript;
    public GameObject RightTree;
    public GameObject LeftTree;
    public GameObject Entry;

    private bool isUsed = false;

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
        if (!isUsed && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("FallParticleTrigger & Player");

            entry Entryclass = Entry.GetComponent<entry>();
            Entryclass.entry_on = true;

            CameraMgrScript.ShakingCameraOn();
            MovingTree Rtreeclass = RightTree.GetComponent<MovingTree>();
            MovingTree Ltreeclass = LeftTree.GetComponent<MovingTree>();

            Rtreeclass.entry_on = true;
            Ltreeclass.entry_on = true;

            isUsed = true;
        }
    }
}
