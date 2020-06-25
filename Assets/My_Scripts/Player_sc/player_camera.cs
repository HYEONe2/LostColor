using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.position;

        transform.LookAt(new Vector3(targetPos.x, targetPos.y + 2f, targetPos.z));
        transform.position = new Vector3(targetPos.x, targetPos.y + 4f, targetPos.z - 5f);
    }
}