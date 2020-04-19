using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text m_textDamage = null;

    public void Play(int nDamage)
    {
        m_textDamage      = transform.GetChild(0).GetComponent<Text>();
        m_textDamage.text = nDamage.ToString();

        iTween.MoveBy(gameObject, new Vector3(0.0f, 2.0f, 0.0f), 1.0f);
        
        StartCoroutine(StartEffect());
    }

    private IEnumerator StartEffect()
    {
        bool  isLoop     = true;
        float fStartTime = 0.0f;

        fStartTime = Time.time;

        while (isLoop)
        {
            yield return new WaitForFixedUpdate();

            float fRate       = 0.0f;
            float fTimePassed = 0.0f;

            fTimePassed         = Time.time - fStartTime;
            fRate               = fTimePassed / 1.5f;
            m_textDamage.color  = new Color(1.0f, 0.0f, 0.0f, 1.0f - fRate);

            if (fTimePassed > 1.5f)
            {
                isLoop = false;

                DamageTextPool.GetInstance().ReleaseObject(gameObject);
            }
        }
    }
}
