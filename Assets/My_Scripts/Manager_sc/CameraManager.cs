using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private GameObject MainCamera;
    private GameObject ShakingCamera;
    private GameObject ShakingCamera2;
    private GameObject ShakingCamera3;
    private GameObject ClearCamera1;
    private GameObject ClearCamera2;
    private GameObject ClearCamera3;

    private ShakingCamera ShakeCameraSc;
    private ShakingCamera ShakeCameraSc2;
    private ShakingCamera ShakeCameraSc3;
    private ClearCamera_sc ClearCameraSc1;
    private ClearCamera_sc ClearCameraSc2;
    private ClearCamera_sc ClearCameraSc3;

    float ShakeMaxTime = 5f;
    float ClearMaxTime = 15f;
    float Magnitude = 5f;

    public bool m_bChangeSkyBox = false;
    private Material SkyboxMaterial;

    private bool m_bClearCameraOn = false;
    public bool GetClearCameraOn() { return m_bClearCameraOn; }

    // Start is called before the first frame update
    void Start()
    {
        // MaxTime = 5f;
        //Magnitude = 10f;
        MainCamera = GameObject.Find("MainCamera");
        ShakingCamera = GameObject.Find("Shaking Camera");
        ShakingCamera2 = GameObject.Find("Shaking Camera2");
        ShakingCamera3 = GameObject.Find("Shaking Camera3");
        ClearCamera1 = GameObject.Find("ClearCamera1");
        ClearCamera2 = GameObject.Find("ClearCamera2");
        ClearCamera3 = GameObject.Find("ClearCamera3");

        ShakeCameraSc = ShakingCamera.GetComponent<ShakingCamera>();
        ShakeCameraSc2 = ShakingCamera2.GetComponent<ShakingCamera>();
        ShakeCameraSc3 = ShakingCamera3.GetComponent<ShakingCamera>();
        ClearCameraSc1 = ClearCamera1.GetComponent<ClearCamera_sc>();
        ClearCameraSc2 = ClearCamera2.GetComponent<ClearCamera_sc>();
        ClearCameraSc3 = ClearCamera3.GetComponent<ClearCamera_sc>();

        SkyboxMaterial = Resources.Load<Material>("Skybox Cubemap Extended Day");

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

        if (Input.GetKey("4"))
            ClearCameraOn();

        if (Input.GetKey("5"))
            ClearCamera3On();
    }

    public void MainCameraOn()
    {
        m_bClearCameraOn = false;
        MainCamera.GetComponent<Camera>().enabled = true;

        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        if (m_bChangeSkyBox)
            RenderSettings.skybox = SkyboxMaterial;
    }

    public void ShakingCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = true;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        StartCoroutine(ShakeCameraSc.Shake(ShakeMaxTime, Magnitude));
    }

    public void ShakingCamera2On()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = true;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        StartCoroutine(ShakeCameraSc2.Shake(ShakeMaxTime, Magnitude));
    }

    public void ShakingCamera3On()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = true;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        StartCoroutine(ShakeCameraSc3.Shake(ShakeMaxTime, Magnitude));
    }

    public void ClearCameraOn()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = true;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        m_bClearCameraOn = true;
        StartCoroutine(ClearCameraSc1.Clear(ClearMaxTime, Magnitude));
    }

    public void ClearCamera2On()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = true;
        ClearCamera3.GetComponent<Camera>().enabled = false;

        m_bClearCameraOn = true;
        StartCoroutine(ClearCameraSc2.Clear(ClearMaxTime, Magnitude));
    }

    public void ClearCamera3On()
    {
        MainCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera.GetComponent<Camera>().enabled = false;
        ShakingCamera2.GetComponent<Camera>().enabled = false;
        ShakingCamera3.GetComponent<Camera>().enabled = false;
        ClearCamera1.GetComponent<Camera>().enabled = false;
        ClearCamera2.GetComponent<Camera>().enabled = false;
        ClearCamera3.GetComponent<Camera>().enabled = true;

        m_bClearCameraOn = true;
        StartCoroutine(ClearCameraSc3.Clear(ClearMaxTime, Magnitude));
    }
}
