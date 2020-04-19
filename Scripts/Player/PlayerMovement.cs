using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected Animator m_matorCharacter   = null;
    protected PlayerAttack m_playerAttack = null;

    private float m_fLastAttackTime = 0.0f;
    private float m_fLastSkillTime  = 0.0f;
    private float m_fLastDashTime   = 0.0f;

    // x-axis의 이동 값에 관여합니다.
    private float m_fControllerHorizontal = 0.0f;
    // z-axis의 이동 값에 관여합니다.
    private float m_fControllerVertical   = 0.0f;

    public bool m_isAttacking    = false;
    public bool m_isDashing      = false;
    public float m_fFallingSpeed = 0.01f;

    public static PlayerMovement m_refInstance = null;

    private void Awake()
    {
        m_refInstance    = this;
        m_matorCharacter = GetComponent<Animator>();
        m_playerAttack   = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (!m_matorCharacter)
            return;

        m_matorCharacter.SetFloat
        (
            "Speed",
            C_SimpleMath.GetSpeed(ref m_fControllerHorizontal, ref m_fControllerVertical)
        );

        if (PlayerCollider.GetInstance() && PlayerCollider.GetInstance().IsFalling())
        {
            transform.position += new Vector3(0.0f, -m_fFallingSpeed, 0.0f);

            return;
        }

        if (m_fControllerHorizontal != 0.0f && m_fControllerVertical != 0.0f)
        {
            Vector3 vecLook = new Vector3(m_fControllerHorizontal, 0.0f, m_fControllerVertical);

            transform.rotation = Quaternion.LookRotation(vecLook);
        }
    }

    static public PlayerMovement GetInstance()
    {
        return m_refInstance;
    }

    private IEnumerator StartAttack()
    {
        if (Time.time - m_fLastAttackTime > 1.0f)
        {
            m_fLastAttackTime = Time.time;

            while (m_isAttacking)
            {
                m_matorCharacter.SetTrigger("AttackStart");

                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void OnStickChanged(Vector2 vecStickpos)
    {
        m_fControllerHorizontal = vecStickpos.x;
        m_fControllerVertical   = vecStickpos.y;
    }

    public void OnAttackDown()
    {
        m_isAttacking = true;

        m_matorCharacter.SetBool("Combo", true);

        StartCoroutine(StartAttack());
    }

    public void OnAttackUp()
    {
        m_isAttacking = false;

        m_matorCharacter.SetBool("Combo", false);
        m_matorCharacter.ResetTrigger("AttackStart");
    }

    public void OnSkillDown()
    {
        if(Time.time - m_fLastSkillTime > 3.0f)
        {
            m_fLastSkillTime = Time.time;

            m_matorCharacter.SetBool("Skill", true);
        }
    }

    public void OnSkillUp()
    {
        m_matorCharacter.SetBool("Skill", false);
    }

    public void OnDashDown()
    {
        if(Time.time - m_fLastDashTime > 1.0f)
        {
            m_isDashing     = true;
            m_fLastDashTime = Time.time;

            m_matorCharacter.SetTrigger("Dash");
        }
    }

    public void OnDashUp()
    {
        m_isDashing = false;

        m_matorCharacter.ResetTrigger("Dash");
    }
}
