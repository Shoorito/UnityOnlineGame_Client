using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTarget : MonoBehaviour
{
    public List<Collider> m_listTargets = null;

    private static SkillTarget m_refInstance = null;

    public static SkillTarget GetInstance()
    {
        return m_refInstance;
    }

    private void Awake()
    {
        m_refInstance = this;
        m_listTargets = new List<Collider>();
    }

    private void Update()
    {
        if (m_listTargets.Count < 1)
            return;

        bool isArrayEnd = false;

        for(int nOut = 0; !isArrayEnd;)
        {
            if (nOut == m_listTargets.Count)
                return;

            if(m_listTargets[nOut].GetComponent<EnemyHealth>().IsDead())
            {
                isArrayEnd = (nOut + 1 == m_listTargets.Count);

                OnTriggerExit(m_listTargets[nOut]);
            }
            else
            {
                nOut++;
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag != "Enemy")
        {
            return;
        }

        if (collider.GetComponent<EnemyHealth>().IsDead())
        {
            OnTriggerExit(collider);

            return;
        }

        m_listTargets.Add(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        m_listTargets.Remove(collider);
    }
}
