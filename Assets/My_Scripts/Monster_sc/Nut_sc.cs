using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;
    Vector3 vDir;

    public bool m_bPlayerUse = false;

    //private Vector3 CameraDir;
    float fSpeed = 5.0f;

   // [SerializeField] private GameObject Blood;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;//new Vector3(Target.transform.position.x, Target.transform.position.y-1.0f, Target.transform.position.z);
        vDir = (TargetPos - gameObject.transform.position).normalized;
        Destroy(gameObject, 2);

        if (m_bPlayerUse)
        {
            Vector3 dir = GameObject.Find("MainCamera").transform.forward;
            transform.position = new Vector3(TargetPos.x + dir.x * 3f, TargetPos.y + 1f, TargetPos.z + dir.z * 3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bPlayerUse)
        {
            Vector3 dir = GameObject.Find("MainCamera").transform.forward;
            gameObject.transform.position += dir * Time.deltaTime * fSpeed;
        }
        else
            gameObject.transform.position += vDir * Time.deltaTime * fSpeed;      
        //Vector3 vPlayerTrans = Target.transform.position; ;
        //float fDist = (TargetPos - gameObject.transform.position).magnitude;
    }
}
