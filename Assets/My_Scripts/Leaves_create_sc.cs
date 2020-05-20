using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves_create_sc : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody.AddForce(transform.forward, ForceMode.Impulse);
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
