using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut_sc : MonoBehaviour
{

    GameObject Target;
    Vector3 TargetPos;
    Vector3 vDir;

    //private Vector3 CameraDir;
    float fSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;//new Vector3(Target.transform.position.x, Target.transform.position.y-1.0f, Target.transform.position.z);
        vDir = (TargetPos - gameObject.transform.position).normalized;
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 vPlayerTrans = Target.transform.position; ;

        //float fDist = (TargetPos - gameObject.transform.position).magnitude;
            gameObject.transform.position += vDir * Time.deltaTime * fSpeed;      
    }
}
