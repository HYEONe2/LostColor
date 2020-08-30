using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LodingSceneManager_sc : MonoBehaviour
{
    public static string curScene;
    public static string nextScene;
    public GameObject loadingText;
   // private Touch touch = Input.GetTouch(0);

    [SerializeField]
    Image progressBar;

    CameraManager CameraManager;
    Stage_Manager StageManager;

    private void Start()
    {
        StartCoroutine(LoadScene());
        loadingText.SetActive(false);

        if(GameObject.Find("CameraManager"))
            CameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        if (!StageManager)
            StageManager = GameObject.Find("StageManager").GetComponent<Stage_Manager>();
    }

    public static void LoadScene(string sceneName)
    {
        curScene = SceneManager.GetActiveScene().name;
        nextScene = sceneName;
        SceneManager.LoadScene("Loading_Scene");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation oper = SceneManager.LoadSceneAsync(nextScene);

        oper.allowSceneActivation = false;
        //allowSceneActivation 이 true가 되는 순간이 바로 다음 씬으로 넘어가는 시점

        float timer = 0.0f;
        while (!oper.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (oper.progress >= 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f && timer > 2.0f)
                {     
                    loadingText.SetActive(true);
                   // if (Input.GetMouseButton(0))
                        oper.allowSceneActivation = true;

                    if (CameraManager && nextScene == "MainStage")
                    {
                        if (curScene == "Stage_1")
                        {
                            if (!StageManager.stage1_open)
                                CameraManager.ClearCameraOn();
                        }
                        else if (curScene == "Stage_2")
                        {
                            if (!StageManager.stage2_open)
                                CameraManager.ClearCamera2On();
                        }
                    }
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, oper.progress, timer);
                if (progressBar.fillAmount >= oper.progress)
                {
                    timer = 0f;
                }
            }
        }
    }
}
