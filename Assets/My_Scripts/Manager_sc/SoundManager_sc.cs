using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager_sc : MonoBehaviour
{
    private Stage_Manager StageManager;
    private bool bIsPlay = false;
    
        // Start is called before the first frame update
    void Start()
    {
        StageManager = GameObject.Find("StageManager").GetComponent<Stage_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bIsPlay)
        {
            if (!StageManager.stage1_open && !StageManager.stage2_open)
            {

                gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/Ending");
                gameObject.GetComponent<AudioSource>().Play();
                bIsPlay = true;
            }       
        }
    }
}
