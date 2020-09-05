using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;
    Vector3 vDir;

    public bool m_bPlayerUse = false;
    Vector3 dir;

    //private Vector3 CameraDir;
    float fSpeed = 5.0f;

    [SerializeField] private GameObject Blood;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;//new Vector3(Target.transform.position.x, Target.transform.position.y-1.0f, Target.transform.position.z);
        vDir = (TargetPos - gameObject.transform.position).normalized;
        Destroy(gameObject, 2);

        if (m_bPlayerUse)
        {
            dir = GameObject.Find("MainCamera").transform.forward;
            transform.Rotate(new Vector3(0, 0, 3));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bPlayerUse)
            gameObject.transform.position += dir * Time.deltaTime * fSpeed * 1.5f;
        else
            gameObject.transform.position += vDir * Time.deltaTime * fSpeed;      

        //Vector3 vPlayerTrans = Target.transform.position; ;
        //float fDist = (TargetPos - gameObject.transform.position).magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_bPlayerUse)
        {
            if (other.gameObject.CompareTag("Boss"))
            {
                Vector3 OriginPos = gameObject.transform.position;
                Instantiate(Blood, OriginPos, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
