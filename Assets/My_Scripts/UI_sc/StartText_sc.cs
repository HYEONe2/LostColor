using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartText_sc : MonoBehaviour
{
    TextMeshProUGUI text;
    bool bIsInit = false;
    float StartTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        StartTime += Time.deltaTime;
        if (StartTime > 1.0f)
        {
            if (!bIsInit)
            {
                StartCoroutine(FadeTextToZero());
                bIsInit = true;
            }
        }
    }

    public IEnumerator FadeTextToFullAlpha() // 알파값 0에서 1로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 1.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToZero());
    }
    public IEnumerator FadeTextToZero()  // 알파값 1에서 0으로 전환
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 1.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha());
    }
}
