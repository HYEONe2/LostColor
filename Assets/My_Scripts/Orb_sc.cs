using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_sc : MonoBehaviour
{
    private GameObject Player;
    private Vector3 PlayerPos;

    private Vector3 CameraDir;
    private float fSpeed = 10;

    bool bReturn = false;
    int iDamage = 5;

    [SerializeField] private GameObject Blood;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PlayerPos = Player.transform.position;

        CameraDir = GameObject.Find("MainCamera").transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if(!bReturn)
        {
            float fDist = (PlayerPos - gameObject.transform.position).magnitude;

            if (fDist < 10)
                gameObject.transform.position += CameraDir * Time.deltaTime * fSpeed;
            else
                bReturn = true;
        }
        else
        {
            Vector3 vPlayerTrans = Player.transform.position;
            float fDist = (vPlayerTrans - gameObject.transform.position).magnitude;

            if (fDist < 1.5f)
                Destroy(gameObject);

            vPlayerTrans.y += 1.0f;
            Vector3 vDir = (vPlayerTrans - gameObject.transform.position).normalized;
            gameObject.transform.position += vDir * Time.deltaTime * fSpeed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
        {
            //other.gameObject.SetDamage(iDamage);
            Vector3 OriginPos = gameObject.transform.position;
            Instantiate(Blood, OriginPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
