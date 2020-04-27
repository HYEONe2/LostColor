using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTree : MonoBehaviour
{
    public bool IsRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move_Tree(float fMaxTime)
    {
        float fTimer = 0f;

        Vector3 vDir = transform.forward;
        vDir.Normalize();

        while (fTimer < fMaxTime)
        {
            if (!IsRight)
            {
                Debug.Log("YES");
                transform.position += vDir * Time.deltaTime;
            }
            else
            {
                transform.position -= vDir * Time.deltaTime;
            }

            fTimer += Time.deltaTime;
        }
    }
}
