using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ShakingCamera;
    public ShakingCamera ShakeCameraScript;

    public float MaxTime;
    public float Magnitude;

    // Start is called before the first frame update
    void Start()
    {
        MaxTime = 5f;
        Magnitude = 10f;

        MainCameraOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("1"))
            MainCameraOn();

        if (Input.GetKey("2"))
            ShakingCameraOn();
    }

    public void MainCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = true;
        ShakingCamera.GetComponent<Camera>().enabled = false;
    }

    public void ShakingCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = true;

        StartCoroutine(ShakeCameraScript.Shake(MaxTime,Magnitude));
    }
}
