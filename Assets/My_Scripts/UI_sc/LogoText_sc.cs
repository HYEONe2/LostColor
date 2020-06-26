using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogoText_sc : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] private GameObject start;
    float StartTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        //Start = GameObject.Find("Start");
        start.SetActive(false);
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeTextToFullAlpha());
    }
    void Update()
    {
        if (StartTime <= 3.5f)
            StartTime += Time.deltaTime;
        // Debug.Log(Time.deltaTime);
        if (StartTime > 3.0f)
            start.SetActive(true);

    }

    public IEnumerator FadeTextToFullAlpha() // 알파값 0에서 1로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 3.0f));
            yield return null;
        }
        // StartCoroutine(FadeTextToZeroAlpha());
    }
}
