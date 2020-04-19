using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter
{
    public enum E_SUBJECT
    {
        E_PLAYER_DATA,
        E_MAX
    }

    private static NotificationCenter     m_refInstance = null;
    private Dictionary<E_SUBJECT, Action> m_delegateMap = null;

    public static NotificationCenter GetInstance()
    {
        return m_refInstance;
    }

    public static NotificationCenter Create()
    {
        if (m_refInstance != null)
        {
            Debug.Log("이미 'NotificationCenter'가 생성되어 있습니다.");

            return null;
        }

        m_refInstance               = new NotificationCenter();
        m_refInstance.m_delegateMap = new Dictionary<E_SUBJECT, Action>();

        return m_refInstance;
    }

    public void Add(E_SUBJECT subjectType, Action callback)
    {
        if (m_delegateMap.ContainsKey(subjectType) == false)
        {
            m_delegateMap[subjectType] = delegate () { };
        }

        m_delegateMap[subjectType] += callback;
    }

    public void Delete(E_SUBJECT subjectType, Action callback)
    {
        if (m_delegateMap.ContainsKey(subjectType) == false)
        {
            return;
        }

        m_delegateMap[subjectType] -= callback;
    }

    public void Notify(E_SUBJECT subjectType)
    {
        if (m_delegateMap.ContainsKey(subjectType) == false)
        {
            return;
        }

        foreach (Action delegator in m_delegateMap[subjectType].GetInvocationList())
        {
            try
            {
                delegator();
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Notify 함수에서 콜백 함수에 대한 예외가 발생하였습니다..");
                Debug.LogException(exception);
            }
        }
    }
}