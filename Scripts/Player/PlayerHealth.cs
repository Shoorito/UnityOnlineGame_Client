using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private bool            m_isDamaged      = false;
    private bool            m_isPlayerDead   = false;
    private Animator        m_animPlayer     = null;
    private AudioSource     m_audioPlayer    = null;
    private PlayerMovement  m_movementPlayer = null;

    public int       m_nStartHealth     = 100;
    public int       m_nCurrentHealth   = 0;
    public float     m_fFlashSpeed      = 5.0f;
    public Color     m_colorFlash       = new Color(1.0f, 0.0f, 0.0f, 0.1f);
    public Image     m_imgDamage        = null;
    public Slider    m_silderHealth     = null;
    public AudioClip m_clipDeath        = null;

    // Start is called before the first frame update
    private void Awake()
    {
        m_animPlayer     = GetComponent<Animator>();
        m_audioPlayer    = GetComponent<AudioSource>();
        m_movementPlayer = GetComponent<PlayerMovement>();
        m_nCurrentHealth = UserSingleton.GetInstance().m_nHealth;

        m_silderHealth.value    = m_nCurrentHealth;
        m_silderHealth.maxValue = m_nCurrentHealth;
    }

    private void Update()
    {
        if (m_isPlayerDead)
            return;

        if(m_isDamaged)
        {
            m_isDamaged       = false;
            m_imgDamage.color = m_colorFlash;
        }
        else
        {
            m_imgDamage.color = Color.Lerp(m_imgDamage.color, Color.clear, m_fFlashSpeed * Time.deltaTime);
        }

    }

    public void TakeDamage(int nAmount)
    {
        if (m_isPlayerDead || m_nCurrentHealth <= 0)
            return;

        m_isDamaged           = true;
        m_nCurrentHealth     -= nAmount;
        m_silderHealth.value  = m_nCurrentHealth;

        if(m_nCurrentHealth <= 0)
        {
            Death();
        }
        else
        {
            m_animPlayer.SetTrigger("Damage");
        }
    }

    public ref Animator GetPlayerAnimator()
    {
        return ref m_animPlayer;
    }

    private void Death()
    {
        m_isPlayerDead   = true;
        m_nCurrentHealth = 0;

        m_animPlayer.SetTrigger("Die");
        m_movementPlayer.enabled = false;

        StageController.m_thisInstance.FinishGame();
    }
}
