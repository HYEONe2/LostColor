using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAttack_sc : MonoBehaviour
{
    private float fDisttime = 0.0f;
    public bool m_bPlayerUse = false;

    [SerializeField] private GameObject Effect;
    private bool bEffectOn = true;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_bPlayerUse)
        {
            if (other.gameObject.CompareTag("Boss"))
            {
                gameObject.SetActive(false);
                if (bEffectOn)
                {
                    Instantiate(Effect, transform.position, Quaternion.identity);
                    bEffectOn = false;
                }
            }
        }
    }
}
