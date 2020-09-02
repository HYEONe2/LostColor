using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager_sc : MonoBehaviour
{
    private Stage_Manager StageManager;
    private string curScene;
    private static string nextScene;
    private bool bIsPlay = false;

    public AudioSource SoundAudio;
    private List<AudioClip> Sound = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        StageManager = GameObject.Find("StageManager").GetComponent<Stage_Manager>();

        curScene = "";
        //this.SoundAudio = gameObject.AddComponent<AudioSource>();

        Sound.Add(Resources.Load<AudioClip>("Sound/Background/MainStage"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Background/BossStage"));
        Sound.Add(Resources.Load<AudioClip>("Sound/Background/Ending"));

        SoundAudio.clip = Sound[0];
        SoundAudio.loop = true;
        SoundAudio.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        nextScene = SceneManager.GetActiveScene().name;

        if (curScene == nextScene)
            return;

        curScene = nextScene;
        
         if (!StageManager.stage1_open && !StageManager.stage2_open && !StageManager.stage3_open)
        {
            if (curScene == "Loading_Scene")
            {
                SoundAudio.Stop();
            }
            else
            {
                SoundAudio.clip = Sound[2];
                SoundAudio.Play();
            }
               
        }
        else if (curScene == "MainStage")
        {
            SoundAudio.clip = Sound[0];
            SoundAudio.Play();
        }
        else if (curScene == "Stage_1" || curScene == "Stage_2" || curScene == "Stage_3")
        {
            SoundAudio.clip = Sound[1];
            SoundAudio.Play();
        }
        else if(curScene == "Loading_Scene")
        {
            SoundAudio.Stop();
        }
        else
        {
            SoundAudio.clip = Sound[0];
            SoundAudio.Play();
        }
        //if (!bIsPlay)
        //{
        //    if (!StageManager.stage1_open && !StageManager.stage2_open)
        //    {
        //        gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sound/Background/Ending");
        //        gameObject.GetComponent<AudioSource>().Play();
        //        bIsPlay = true;
        //    }       
        //}
    }
}
