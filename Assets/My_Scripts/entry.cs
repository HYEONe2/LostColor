using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class entry : MonoBehaviour
{
    private float y_pos = 0.0f;
    private float deltatime;
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
        transform.Translate(0.0f, y_pos, 0.0f);

        if (entry_on)
        {
            Debug.Log("입구등장");
            y_pos += speed * Time.deltaTime;
            if (transform.position.y >= 0.0f)
            { 
                y_pos = 0.0f;
            }
        }
    }
}
