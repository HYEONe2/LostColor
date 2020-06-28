using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endind_sc : MonoBehaviour
{ 
    public bool m_bStartEnding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > 1500f)
        {
            SceneManager.LoadScene("Logo_Scene");
            return;
        }

        if (m_bStartEnding)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+Time.deltaTime *150f, 0);
    }
}
