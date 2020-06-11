using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut_sc : MonoBehaviour
{

    GameObject Target;
    Vector3 TargetPos;

    //private Vector3 CameraDir;
    float fSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;//new Vector3(Target.transform.position.x, Target.transform.position.y-1.0f, Target.transform.position.z);
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vPlayerTrans = Target.transform.position; ;

        vPlayerTrans.y += 1.0f;
        Vector3 vDir = (vPlayerTrans - gameObject.transform.position).normalized;
        gameObject.transform.position += vDir * Time.deltaTime * fSpeed;
    }
}
