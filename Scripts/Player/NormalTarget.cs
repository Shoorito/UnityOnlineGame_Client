using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTarget : MonoBehaviour
{
    private List<Collider> m_listTargets = null;

    private void Awake()
    {
        m_listTargets = new List<Collider>();
    }

    public ref List<Collider> GetTargetList()
    {
        return ref m_listTargets;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Enemy")
        {
            m_listTargets.Add(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        m_listTargets.Remove(collider);
    }
}
