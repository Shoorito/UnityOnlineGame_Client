using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack02 : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine(StartDisappearAfter(2.0f));
    }

    private IEnumerator StartDisappearAfter(float fTime)
    {
        SkillAttackPool poolObject = null;

        yield return new WaitForSeconds(fTime);

        poolObject = StageController.m_thisInstance.GetSkillPool((int)PlayerAttack.E_SKILL_TYPE.E_SKILL_02 - 1);

        poolObject.ReleaseObject(gameObject);
    }
}
