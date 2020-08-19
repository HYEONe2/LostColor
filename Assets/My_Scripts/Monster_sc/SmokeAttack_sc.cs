using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAttack_sc : MonoBehaviour
{
    private float fDisttime = 0.0f;

    public bool m_bPlayerUse = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);

        if(m_bPlayerUse)
        {
            Vector3 PlayerPos = GameObject.Find("Player").transform.position;

            Vector3 dir = GameObject.Find("MainCamera").transform.forward;
            transform.position = new Vector3(PlayerPos.x + dir.x * 3f, PlayerPos.y, PlayerPos.z + dir.z * 3f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
