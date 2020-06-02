using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_sc : MonoBehaviour
{
    private Transform PlayerTrans;
    private Transform Trans;

    Vector3 vOriginPos;
    float fSpeed = 60.0f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerTrans= GameObject.Find("Player").transform;
        vOriginPos = PlayerTrans.position;
        Trans = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vDist = PlayerTrans.position - vOriginPos;
        Trans.position += vDist;

        Trans.RotateAround(PlayerTrans.position, Vector3.down, fSpeed * Time.deltaTime);

        vOriginPos = PlayerTrans.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss"))
            Destroy(gameObject);
    }

}
