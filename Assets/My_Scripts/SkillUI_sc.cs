using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillUI_sc : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Player_sc playerScript;
    RectTransform m_rectMainSkill;
    float m_fRadius;

    bool m_bTouch = false;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rectMainSkill = transform.Find("MainAttack").GetComponent<RectTransform>();
        m_fRadius = m_rectMainSkill.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bTouch)
        {
            Debug.Log("WORKING");
            playerScript.MainAttack();
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        //Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, vecTouch.y - m_rectBack.position.y);

        //// vec값을 m_fRadius 이상이 되지 않도록 합니다.
        //vec = Vector2.ClampMagnitude(vec, m_fRadius);
        //m_rectJoystick.localPosition = vec;

        //// 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        //m_fSqr = (m_rectBack.position - m_rectJoystick.position).sqrMagnitude / (m_fRadius * m_fRadius);

        //// 터치위치 정규화
        //Vector2 vecNormal = vec.normalized;

        //m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * m_fSqr, 0f, vecNormal.y * m_fSpeed * Time.deltaTime * m_fSqr);
        //m_playerTrans.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        m_bTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_bTouch = false;
    }
}
