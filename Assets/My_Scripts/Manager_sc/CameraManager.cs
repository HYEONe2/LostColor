using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ShakingCamera;
    public GameObject ShakingCamera2;
    public ShakingCamera ShakeCameraScript;
    public ShakingCamera ShakeCameraScript2;


    public float MaxTime = 5f;
    public float Magnitude = 10f;

    // Start is called before the first frame update
    void Start()
    {
       // MaxTime = 5f;
        //Magnitude = 10f;

        MainCameraOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
            MainCameraOn();

        if (Input.GetKey("2"))
            ShakingCameraOn();

        if (Input.GetKey("3"))
            ShakingCamera2On();
    }

    public void MainCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = true;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
    }

    public void ShakingCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = true;
        ShakingCamera2.GetComponent<Camera>().enabled = false;

        StartCoroutine(ShakeCameraScript.Shake(MaxTime,Magnitude));
    }

    public void ShakingCamera2On()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = true;

        StartCoroutine(ShakeCameraScript2.Shake(MaxTime, Magnitude));
    }
}
