using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingCamera : MonoBehaviour
{
    public CameraManager CameraMgrScript;

    Vector3 vOriginPos;

    void Start()
    {
    }

    public IEnumerator Shake(float fMaxTime, float magnitude)
    {
        Vector3 vOriginPos = transform.position;
        float fTimer = 0f;

        while (fTimer < fMaxTime)
        {
            float fRandomX = Random.Range(-10f, 10f) * magnitude * Time.deltaTime;
            float fRandomY = Random.Range(-10f, 10f) * magnitude * Time.deltaTime;

            fTimer += Time.deltaTime;
            transform.position = new Vector3(vOriginPos.x + fRandomX, vOriginPos.y + fRandomY, vOriginPos.z);

            yield return null;
        }

        transform.position = vOriginPos;

        CameraMgrScript.MainCameraOn();
    }
}
