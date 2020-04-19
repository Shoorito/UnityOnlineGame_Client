using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMover : MonoBehaviour
{
    private Transform m_transPlayer = null;
    private NavMeshAgent m_navEnemy = null;

    void Awake()
    {
        m_navEnemy    = GetComponent<NavMeshAgent>();
        m_transPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(m_navEnemy.enabled)
        {
            m_navEnemy.SetDestination(m_transPlayer.position);
        }
    }
}
