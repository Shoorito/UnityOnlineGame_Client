using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int   m_nAttackDamage        = 10;
    public float m_fTimeBetweenAttacks  = 1.0f;

    private bool         m_isPlayerInRange = false;
    private float        m_fTimer          = 0.0f;
    private Collider     m_colliderPlayer  = null;
    private PlayerHealth m_healthPlayer    = null;
    private EnemyHealth  m_healthEnemy     = null;

    private void Awake()
    {
        m_colliderPlayer = PlayerCollider.GetInstance().GetComponent<Collider>();
        m_healthPlayer   = PlayerMovement.GetInstance().GetComponent<PlayerHealth>();
        m_healthEnemy    = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (m_healthEnemy.GetHP() <= 0 || !m_isPlayerInRange)
            return;

        if (m_fTimer >= m_fTimeBetweenAttacks)
        {
            Attack();
        }

        m_fTimer += Time.deltaTime;
    }

    private void Attack()
    {
        m_fTimer = 0.0f;

        if(m_healthPlayer.m_nCurrentHealth > 0)
        {
            m_healthPlayer.TakeDamage(m_nAttackDamage);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider == m_colliderPlayer)
        {
            m_isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider == m_colliderPlayer)
        {
            m_isPlayerInRange = false;

            m_healthPlayer.GetPlayerAnimator().ResetTrigger("Damage");
        }
    }
}
