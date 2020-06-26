﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick_sc : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Animator m_animator;

    RectTransform m_rectBack;
    RectTransform m_rectJoystick;

    Transform m_playerTrans;
    float m_fRadius;
    float m_fSpeed = 2.0f;
    float m_fSqr = 0f;

    Vector3 m_vecMove;
    Vector2 m_vecNormal;

    bool m_bTouch = false;
    // Start is called before the first frame update
    void Start()
    {
        m_rectBack = transform.Find("JoyStickBack").GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("JoyStickBack/JoyStick").GetComponent<RectTransform>();

        m_animator = GameObject.Find("Player").GetComponent<Animator>();
        m_playerTrans = GameObject.Find("Player").transform;

        // JoystickBackground의 반지름입니다.
        m_fRadius = m_rectBack.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bTouch)
        {
            Debug.Log("LEKFJLEKJ");
            m_playerTrans.position += m_vecMove;

            m_animator.SetFloat("MoveSpeed", m_fSqr);
        }
    }

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 vec = new Vector2(vecTouch.x - m_rectBack.position.x, vecTouch.y - m_rectBack.position.y);

        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, m_fRadius);
        m_rectJoystick.localPosition = vec;

        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        m_fSqr = (m_rectBack.position - m_rectJoystick.position).sqrMagnitude / (m_fRadius * m_fRadius);

        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;
        Vector2 vecDir = (m_rectBack.position - m_rectJoystick.position).normalized;

        m_vecMove = new Vector3(vecNormal.x * m_fSpeed * Time.deltaTime * m_fSqr, 0f, vecNormal.y * m_fSpeed * Time.deltaTime * m_fSqr);
        //m_playerTrans.eulerAngles += new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg * Time.deltaTime, 0f);
        m_playerTrans.forward = new Vector3(-vecDir.x, 0, -vecDir.y);
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
        // 원래 위치로 되돌립니다.
        m_rectJoystick.localPosition = Vector2.zero;
        m_bTouch = false;
    }
}