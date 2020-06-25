using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAttack_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = new Vector3 (Target.transform.position.x, Target.transform.position.y+5.0f, Target.transform.position.z);
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 vPlayerTrans = Target.transform.position;

        //TargetPos.y += 1.0f;

        float fDist = (TargetPos - gameObject.transform.position).magnitude;

        Vector3 vDir = (TargetPos - gameObject.transform.position).normalized;
        if (fDist > 1)
            gameObject.transform.position += vDir * Time.deltaTime * 11.0f;
    }
}
