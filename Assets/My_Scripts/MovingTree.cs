using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTree : MonoBehaviour
{
    public bool IsRight=false;
    private float x_pos = 0.0f;
    //private float deltatime;
    public float speed = 0.005f;
    public bool entry_on = false;

    // Start is called before the first frame update
    void Start()
    {
        entry_on = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, 0.0f ,x_pos);

        if (entry_on)
        {
            if (IsRight)
            {
                x_pos += speed * Time.deltaTime;
                if (transform.position.z >= 2.0f)
                {
                    x_pos = 0.0f;
                }
            }
            else if (!IsRight)
            {
                //Debug.Log("왼쪽" + x_pos);
                x_pos -= speed * Time.deltaTime;
                if (transform.position.z <= -7.3f)
                {
                    x_pos = 0.0f;
                }
            }

        }
    }

    //public void Move_Tree(float fMaxTime)
    //{
    //    float fTimer = 0f;

    //    Vector3 vDir = transform.forward;
    //    vDir.Normalize();

    //    while (fTimer < fMaxTime)
    //    {
    //        if (!IsRight)
    //        {
    //            Debug.Log("YES");
    //            transform.position += vDir * Time.deltaTime;
    //        }
    //        else
    //        {
    //            transform.position -= vDir * Time.deltaTime;
    //        }

    //        fTimer += Time.deltaTime;
    //    }
    //}
}
