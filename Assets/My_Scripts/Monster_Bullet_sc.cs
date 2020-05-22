using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Bullet_sc : MonoBehaviour
{
    float bulletSpeed = 20.0f;
    [SerializeField] private Rigidbody m_rigidBody;

    // Start is called before the first frame update
    private void Start()
    {
        m_rigidBody.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(gameObject, 5);
    }


// Update is called once per frame
    private void Update()
    {
        
    }
}
