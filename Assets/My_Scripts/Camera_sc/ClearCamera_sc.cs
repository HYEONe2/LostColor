using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCamera_sc : MonoBehaviour
{
    private CameraManager CameraMgrScript;

    public bool bEndingClearCamera = false;
    Vector3 vEndingClearRoot;

    private float m_Speed = 4f;
    Vector3 vOriginDir;

    // Start is called before the first frame update
    void Start()
    {
        CameraMgrScript = GameObject.Find("CameraManager").GetComponent<CameraManager>();

        if (bEndingClearCamera)
            vEndingClearRoot = GameObject.Find("EndingClearRoot").transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Clear(float fMaxTime, float magnitude)
    {
        Vector3 vOriginDir = transform.TransformDirection(Vector3.forward);
        float fTimer = 0f;

        while (fTimer < fMaxTime)
        {
            fTimer += Time.deltaTime;
            if (bEndingClearCamera)
                transform.RotateAround(vEndingClearRoot, Vector3.up, Time.deltaTime * m_Speed * 2f);
            else
                transform.Rotate(Vector3.up * Time.deltaTime * m_Speed);

            yield return null;
        }

        transform.forward = vOriginDir;
        CameraMgrScript.MainCameraOn();
    }
}
