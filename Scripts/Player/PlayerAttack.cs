using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public enum E_SKILL_TYPE
    {
        E_SKILL_NONE,
        E_SKILL_01,
        E_SKILL_02,
        E_MAX
    }

    public int m_nNormalDamage  = 10;
    public int m_nSkillDamage   = 30;
    public int m_nDashDamage    = 30;

    public NormalTarget m_normalTarget = null;
    public SkillTarget  m_skillTarget  = null;

    private string[]          m_arrNormalSound = null;
    private string[]          m_arrDashSound   = null;
    private string[]          m_arrSkillSound  = null;
    private AudioSource       m_audioSource    = null;

    static private PlayerAttack m_refInstance = null;
    
    private void Awake()
    {
        m_refInstance       = this;
        m_audioSource       = GetComponent<AudioSource>();
        m_arrDashSound      = new string[3];
        m_arrSkillSound     = new string[6];
        m_arrNormalSound    = new string[6];

        m_arrDashSound[0]   = "VoiceSample/10.attack_A1";
        m_arrDashSound[1]   = "VoiceSample/11.attack_A2";
        m_arrDashSound[2]   = "VoiceSample/12.attack_A3";

        m_arrSkillSound[0]  = "VoiceSample/44.special_attack_X1";
        m_arrSkillSound[1]  = "VoiceSample/45.special_attack_X2";
        m_arrSkillSound[2]  = "VoiceSample/46.special_attack_X3";
        m_arrSkillSound[3]  = "VoiceSample/47.special_attack_X4";
        m_arrSkillSound[4]  = "VoiceSample/48.special_attack_X5";
        m_arrSkillSound[5]  = "VoiceSample/50.special_attack_X7";

        m_arrNormalSound[0] = "VoiceSample/13.attack_B1";
        m_arrNormalSound[1] = "VoiceSample/14.attack_B2";
        m_arrNormalSound[2] = "VoiceSample/15.attack_B3";
        m_arrNormalSound[3] = "VoiceSample/16.attack_C1";
        m_arrNormalSound[4] = "VoiceSample/17.attack_C2";
        m_arrNormalSound[5] = "VoiceSample/18.attack_C3";

        m_nNormalDamage = UserSingleton.GetInstance().m_nDamage;
        m_nDashDamage   = m_nNormalDamage * 2;
        m_nSkillDamage  = m_nNormalDamage * 4;
    }

    private void PlayRandomVoice(ref string[] arrSoundSources)
    {
        int nRandValue = 0;

        nRandValue = Random.Range(0, arrSoundSources.Length);

        m_audioSource.PlayOneShot(Resources.Load(arrSoundSources[nRandValue]) as AudioClip);
    }

    public static PlayerAttack GetInstance()
    {
        return m_refInstance;
    }

    public void NormalAttack()
    {
        List<Collider> listTargets = m_normalTarget.GetTargetList();

        if(listTargets == null)
        {
            Debug.Log("이상하게도 몬스터가 감지되지 않습니다?");

            return;
        }

        foreach(Collider collider in listTargets)
        {
            EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();

            if(enemyHealth != null)
            {
                enemyHealth.Damage
                (
                    m_nNormalDamage,
                    transform.position,
                    4.0f,
                    E_SKILL_TYPE.E_SKILL_NONE,
                    "Audio/Knife_Attack_Effect"
                );

                Debug.Log("Find_Success_Enemy!");
            }
            else
            {
                Debug.Log("Enemy_Health_Missing..");
            }
        }

        PlayRandomVoice(ref m_arrNormalSound);

        Debug.Log("Player_Normal_Attack!");
    }

    public void DashAttack()
    {
        for (int nCollider = 0; nCollider < m_skillTarget.m_listTargets.Count; nCollider++)
        {
             EnemyHealth enemyHealth = m_skillTarget.m_listTargets[nCollider].GetComponent<EnemyHealth>();

             if (enemyHealth != null)
             {
                 enemyHealth.Damage
                 (
                     m_nDashDamage,
                     transform.position,
                     10.0f,
                     E_SKILL_TYPE.E_SKILL_02,
                     "Audio/explosion_enemy"
                 );
             }
        }

        Debug.Log("Used Dash Attack!");
    }

    public void SkillAttack()
    {
        List<Collider> listTargets = m_skillTarget.m_listTargets;

        foreach (Collider collider in listTargets)
        {
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.Damage
                (
                    m_nSkillDamage,
                    transform.position,
                    10.0f,
                    E_SKILL_TYPE.E_SKILL_01,
                    "Audio/explosion_player"
                );
            }
            else
            {
                Debug.Log("Enemy_Health_Missing..");
            }
        }
    }
}
