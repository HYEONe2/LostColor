using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_sc : MonoBehaviour
{
    private ParticleSystem ps;

    GameObject Player;
    public bool m_bPlayerUse = false;
    private Vector3 CameraDir;
    float fSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 3);

        Player = GameObject.Find("Player");
        CameraDir = GameObject.Find("MainCamera").transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameObject.transform.position.x + "  " + gameObject.transform.position.z);

        //if (m_bPlayerUse)
        //{
        //    Vector3 vPlayerTrans = Player.transform.position;
        //    float fDist = (vPlayerTrans - gameObject.transform.position).magnitude;

        //    if (fDist > 10.0f)
        //        Destroy(gameObject);

        //    gameObject.transform.position += CameraDir * Time.deltaTime * fSpeed;
        //}
    }
}
