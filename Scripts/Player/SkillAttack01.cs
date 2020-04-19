using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttack01 : MonoBehaviour
{
    public void Play()
    {
        Debug.Log("Call_Skill_Attack_01");

        StartCoroutine(StartDisappearAfter(2.0f));
    }

    private IEnumerator StartDisappearAfter(float fTime)
    {
        SkillAttackPool poolObject = null;

        yield return new WaitForSeconds(fTime);

        poolObject = StageController.m_thisInstance.GetSkillPool((int)PlayerAttack.E_SKILL_TYPE.E_SKILL_01 - 1);

        poolObject.ReleaseObject(gameObject);
    }
}
