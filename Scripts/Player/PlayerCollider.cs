using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public List<Collider> m_listTargets = null;

    static private PlayerCollider m_refInstance = null;

    static public PlayerCollider GetInstance()
    {
        return m_refInstance;
    }

    public bool IsFalling()
    {
        return m_listTargets.Count == 0;
    }

    private void Awake()
    {
        m_refInstance = this;
        m_listTargets = new List<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Land")
        {
            m_listTargets.Add(collider);
        }

        Debug.Log("OnTriggerEnter!");
    }

    private void OnTriggerExit(Collider collider)
    {
        m_listTargets.Remove(collider);
    }
}
