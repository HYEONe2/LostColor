using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Stage_Manager : MonoBehaviour
{
    private static Stage_Manager instance;

    public GameObject paticle1;
    public GameObject paticle2;
    public GameObject paticle3;

    [SerializeField] private GameObject Player;

    public bool stage1_open = true;
    public bool stage2_open = false;
    public bool stage3_open = false;

    public static Stage_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<Stage_Manager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("Stage_Manager").AddComponent<Stage_Manager>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<Stage_Manager>();
        Player = GameObject.Find("Player");
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainStage")
        {
            if (stage1_open)
                paticle1.SetActive(true);
            if (stage2_open)
                paticle2.SetActive(true);
            if (stage3_open)
                paticle3.SetActive(true);
            if (!stage1_open)
                paticle1.SetActive(false);
            if (!stage2_open)
                paticle2.SetActive(false);
            if (!stage3_open)
                paticle3.SetActive(false);
        }
        if(SceneManager.GetActiveScene().name == "Loading_Scene")
        {
           // Player.SetActive(false);
            paticle1.SetActive(false);
            paticle2.SetActive(false);
            paticle3.SetActive(false);
        }
        else
        {
            //Player.SetActive(true);
            //paticle1.SetActive(false);
            //paticle2.SetActive(false);       
        }
    }
}
