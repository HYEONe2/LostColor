using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut_Attack_sc : MonoBehaviour
{
    float fAngle = 0.0f;

    [SerializeField] private GameObject Blood;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fAngle += 1.0f;
        transform.RotateAround(gameObject.transform.position, gameObject.transform.position, fAngle);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 OriginPos = gameObject.transform.position;
            Instantiate(Blood, OriginPos, Quaternion.identity);
            gameObject.SetActive(false);
        }

    }
}
