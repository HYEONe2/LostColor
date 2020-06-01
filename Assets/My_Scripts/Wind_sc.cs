using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_sc : MonoBehaviour
{
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
