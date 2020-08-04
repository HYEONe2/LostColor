using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;

    public bool m_bPlayerUse = false;
    private GameObject Monster;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;
        Destroy(gameObject, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        float fDist = (TargetPos - gameObject.transform.position).magnitude;

        Vector3 vDir = (TargetPos - gameObject.transform.position).normalized;
        if (fDist > 1)
            gameObject.transform.position += vDir * Time.deltaTime * 11.0f;
    }
}
