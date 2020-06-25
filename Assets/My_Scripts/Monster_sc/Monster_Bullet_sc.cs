using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;

    private Vector3 CameraDir;
    float fSpeed = 10.0f;

    [SerializeField] private GameObject Blood;
    //[SerializeField] private Rigidbody m_rigidBody;

    public bool m_bPlayerUse = false;

    // Start is called before the first frame update
    private void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;
        Destroy(gameObject, 2);

        CameraDir = GameObject.Find("MainCamera").transform.forward;
    }


    // Update is called once per frame
    private void Update()
    {
        if (!m_bPlayerUse)
        {
            Vector3 vPlayerTrans = Target.transform.position;

            vPlayerTrans.y += 1.0f;
            Vector3 vDir = (vPlayerTrans - gameObject.transform.position).normalized;
            gameObject.transform.position += vDir * Time.deltaTime * fSpeed;
        }
        else
        {
            Vector3 vPlayerTrans = Target.transform.position;
            float fDist = (vPlayerTrans - gameObject.transform.position).magnitude;

            if (fDist > 10.0f)
                Destroy(gameObject);

            gameObject.transform.position += CameraDir * Time.deltaTime * fSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_bPlayerUse)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Vector3 OriginPos = gameObject.transform.position;
                Instantiate(Blood, OriginPos, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            if(other.gameObject.CompareTag("Boss"))
            {
                Vector3 OriginPos = gameObject.transform.position;
                Instantiate(Blood, OriginPos, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
