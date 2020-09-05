using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudAttack_sc : MonoBehaviour
{
    GameObject Target;
    Vector3 TargetPos;

    public bool m_bPlayerUse = false;
    Vector3 dir;

    private GameObject Monster;
    [SerializeField] private GameObject rain;
    [SerializeField] private Collider Coll;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = new Vector3 (Target.transform.position.x, Target.transform.position.y+5.0f, Target.transform.position.z);
        Destroy(gameObject, 2.5f);
        rain.SetActive(false);
        Coll.enabled = false;

        if(m_bPlayerUse)
        {
            Monster = GameObject.FindWithTag("Boss");
            if (Monster)
                TargetPos = new Vector3(Monster.transform.position.x, Monster.transform.position.y + 7.0f, Monster.transform.position.z);
            else
            {
                dir = GameObject.Find("MainCamera").transform.forward;
                transform.position = TargetPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 vPlayerTrans = Target.transform.position;
        //TargetPos.y += 1.0f;
        if (m_bPlayerUse)
        {
            if (!Monster)
            {
                gameObject.transform.position += dir * Time.deltaTime * 10f;
            }
            else
            {
                float fDist = (TargetPos - gameObject.transform.position).magnitude;

                Vector3 vDir = (TargetPos - gameObject.transform.position).normalized;
                if (fDist > 1)
                    gameObject.transform.position += vDir * Time.deltaTime * 11.0f;

                else if (rain.activeSelf == false)
                {
                    rain.SetActive(true);
                    Coll.enabled = true;
                }
            }
        }
        else
        {
            float fDist = (TargetPos - gameObject.transform.position).magnitude;

            Vector3 vDir = (TargetPos - gameObject.transform.position).normalized;
            if (fDist > 1)
                gameObject.transform.position += vDir * Time.deltaTime * 11.0f;

            else if (rain.activeSelf == false)
            {
                rain.SetActive(true);
                Coll.enabled = true;
            }
        }
    }
}
