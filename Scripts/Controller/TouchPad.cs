using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchPad : MonoBehaviour
{
    private int           m_nTouchId         = -1;
    private bool          m_isButtonPressed  = false;
    private Vector3       m_vecStartPosition = Vector3.zero;
    private RectTransform m_transTouchPad    = null;

    public float          m_fDragRadius = 0.0f;
    public PlayerMovement m_movement    = null;

    private void Awake()
    {
        m_transTouchPad     = GetComponent<RectTransform>();
        m_vecStartPosition  = m_transTouchPad.position;
        m_fDragRadius       = 60.0f;
    }

    private void Update()
    {
        
    }

    public void ButtonDown()
    {
        m_isButtonPressed = true;
    }

    public void ButtonUp()
    {
        m_isButtonPressed = false;

        HandleInput(m_vecStartPosition);
    }

    private void FixedUpdate()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

        HandleInput(Input.mousePosition);

        #else

        HandleTouchInput();

        #endif
    }

    private void HandleTouchInput()
    {
        int nTouchId = 1;

        if(Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                Vector3 vecTouchpos = new Vector3(touch.position.x, touch.position.y);

                if(touch.phase == TouchPhase.Began && touch.position.x <= (m_vecStartPosition.x + m_fDragRadius))
                {
                    m_nTouchId = nTouchId;
                }

                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if(m_nTouchId == nTouchId)
                    {
                        HandleInput(vecTouchpos);
                    }
                }

                if(touch.phase == TouchPhase.Ended)
                {
                    if(m_nTouchId == nTouchId)
                    {
                        m_nTouchId = -1;
                    }
                }

                nTouchId++;
            }
        }
    }

    private void HandleInput(Vector3 vecInput)
    {
        Vector3 vecDiff     = Vector3.zero;
        Vector2 vecNormDiff = Vector2.zero;

        if (m_isButtonPressed)
        {
            vecDiff = vecInput - m_vecStartPosition;

            if(vecDiff.sqrMagnitude > C_SimpleMath.Square(m_fDragRadius))
            {
                vecDiff.Normalize();

                m_transTouchPad.position  = vecDiff * m_fDragRadius;
                m_transTouchPad.position += m_vecStartPosition;
            }
            else
            {
                m_transTouchPad.position = vecInput;
            }
        }
        else
        {
            m_transTouchPad.position = m_vecStartPosition;
        }

        vecDiff     = m_transTouchPad.position - m_vecStartPosition;
        vecNormDiff = new Vector3(vecDiff.x / m_fDragRadius, vecDiff.y / m_fDragRadius);

        if(m_movement)
        {
            m_movement.OnStickChanged(vecNormDiff);
        }
    }
}
