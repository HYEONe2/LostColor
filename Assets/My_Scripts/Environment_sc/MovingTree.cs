using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTree : MonoBehaviour
{
    public bool IsRight=false;
    private float z_pos = 0.0f;
    private float x_pos = 0.0f;

    //private float deltatime;
    public float speed = 0.005f;
    public bool entry_on = false;
    public bool entry_on2 = false;
    public bool entry_on3 = false;



    // Start is called before the first frame update
    void Start()
    {
        entry_on = false;
        entry_on2 = false;
        entry_on3 = false;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(x_pos, 0.0f ,z_pos);

        if (entry_on)
        {
            First_Entry();
        }

        if(entry_on2)
        {
            Second_Entry();
        }
        if(entry_on3)
        {
            Third_Entry();
        }
    }

    private void First_Entry()
    {
        x_pos = 0.0f;

        if (IsRight)
        {
            z_pos += speed * Time.deltaTime;
            if (transform.position.z >= 2.0f)
            {
                z_pos = 0.0f;
            }
        }
        else if (!IsRight)
        {
            z_pos -= speed * Time.deltaTime;
            if (transform.position.z <= -7.3f)
            {
                z_pos = 0.0f;
            }
        }
    }
    private void Second_Entry()
    {
        z_pos = 0.0f;
        if (IsRight)
        {
            x_pos += speed * Time.deltaTime;
            if (transform.position.x >= 42.6f)
            {
                x_pos = 0.0f;
            }
        }
        else if (!IsRight)
        {
            //Debug.Log("왼쪽" + transform.position.x);
            x_pos -= speed * Time.deltaTime;
            if (transform.position.x <= 31.7f)
            {
                x_pos = 0.0f;
            }
        }
    }

    private void Third_Entry()
    {
        z_pos = 0.0f;
        if (IsRight)
        {
            x_pos -= speed * Time.deltaTime;
            Debug.Log("오른쪽" + transform.position.x);
            if (transform.position.x <= 42.3f)
            {
                x_pos = 0.0f;
            }
        }
        else if (!IsRight)
        {
            Debug.Log("왼쪽" + transform.position.x);
            x_pos += speed * Time.deltaTime;
            if (transform.position.x >= 48.2f)
            {
                x_pos = 0.0f;
            }
        }
    }
}
