using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public int   m_nStartHealth   = 100;
    public int   m_nCurrentHealth = 0;
    public float m_fFlashSpeed    = 5.0f;
    public float m_fSinkSpeed     = 1.0f;
    public Color m_colorFlash     = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    
    public bool m_isDead         = false;
    private bool m_isSinking      = false;
    private bool m_isDamaged      = false;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if(m_isDamaged)
        {
            transform.GetChild(0).GetComponent<Renderer>().material.color = m_colorFlash;
        }
        else
        {
            Color colorValue = transform.GetChild(0).GetComponent<Renderer>().material.color;

            transform.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(colorValue, Color.white, m_fFlashSpeed * Time.deltaTime);
        }

        m_isDamaged = false;

        if(m_isSinking && transform.localPosition.y <= 100.0f)
        {
            transform.Translate(-Vector3.up * m_fSinkSpeed * Time.deltaTime);
        }
    }

    public void Init()
    {
        BoxCollider collider = null;

        m_isDead            = false;
        m_isSinking         = false;
        m_isDamaged         = false;

        m_nCurrentHealth    = m_nStartHealth;

        collider            = transform.GetComponentInChildren<BoxCollider>();
        collider.isTrigger  = false;

        GetComponent<NavMeshAgent>().enabled = true;
    }

    public void Damage
    (
        int nDamage,
        Vector3 vecPlayerPosition,
        float fForce,
        PlayerAttack.E_SKILL_TYPE eType,
        string strAudioSource  = ""
    )
    {
        if (m_isDead)
            return;

        TakeDamage(nDamage);
        ShowDamageText(nDamage);
        AddForce(ref vecPlayerPosition, ref fForce);
        DamageEffect(eType);
        PlaySound(strAudioSource);
    }

    public void SetAlive(bool isAlive)
    {
        m_isDead = !isAlive;
    }

    public int GetHP()
    {
        return m_nCurrentHealth;
    }

    public bool IsDead()
    {
        return m_isDead;
    }

    private void TakeDamage(int nAmount)
    {
        if (m_isDead)
            return;

        m_isDamaged       = true;
        m_nCurrentHealth -= nAmount;

        if(m_nCurrentHealth <= 0)
        {
            Death();

            Debug.Log("MonsterDead");
        }
    }

    private void AddForce(ref Vector3 vecPlayerPosition, ref float fForce)
    {
        Vector3 vecDiff = Vector3.zero;

        vecDiff = vecPlayerPosition - transform.position;
        vecDiff /= vecDiff.sqrMagnitude;

        GetComponent<Rigidbody>().AddForce(vecDiff * -10000.0f * fForce);
    }

    private void ShowDamageText(int nDamage)
    {
        GameObject objDamageFont = null;

        objDamageFont = DamageTextPool.GetInstance().EnableObject();
        objDamageFont.transform.position = transform.position + new Vector3(0.0f, 0.5f, -0.5f);

        objDamageFont.GetComponent<DamageText>().Play(nDamage);
    }

    private void DamageEffect(PlayerAttack.E_SKILL_TYPE eType)
    {
        if (eType == PlayerAttack.E_SKILL_TYPE.E_SKILL_NONE)
            return;

        GameObject      objEffect  = null;
        SkillAttackPool poolObject = null;

        poolObject = StageController.m_thisInstance.GetSkillPool((int)eType - 1);
        objEffect  = poolObject.EnableObject();

        objEffect.transform.position = transform.position + new Vector3(0.0f, 0.5f, -0.5f);
    }

    private void PlaySound(string strAudio)
    {
        if (strAudio == "")
            return;

        AudioSource.PlayClipAtPoint(Resources.Load(strAudio) as AudioClip, transform.position, 0.1f);
    }

    private void Death()
    {
        BoxCollider collider = null;

        m_isDead            = true;
        m_isSinking         = true;
        collider            = transform.GetComponentInChildren<BoxCollider>();
        collider.isTrigger  = true;

        GetComponent<NavMeshAgent>().enabled = false;

        StageController.m_thisInstance.AddPoint(10);

        StartCoroutine(StartRelease(2.0f));
    }

    private IEnumerator StartRelease(float fTime)
    {
        yield return new WaitForSeconds(fTime);

        SlimePool.GetInstance().ReleaseObject(gameObject);
    }
}
