using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillAttackPool : ObjectPool
{
    public static SkillAttackPool Create
    (
        string strPrefabName, // "해당 'Pool'에서 관리 할 프리팹의 이름"
        string strObjectName, // "'PoolObject'의 이름"
        int nPoolItemMaxCount // "'Pool'에서 관리 할 오브젝트의 최고 개수"
    )
    {
        GameObject      objContainer = null;
        SkillAttackPool refResult    = null;

        objContainer = new GameObject();
        refResult    = objContainer.AddComponent(typeof(SkillAttackPool)) as SkillAttackPool;

        refResult.m_strPrefabName   = strPrefabName;
        refResult.m_nObjectMaxCount = nPoolItemMaxCount;
        refResult.gameObject.name   = strObjectName;

        refResult.PreloadObject();

        return refResult;
    }

    public override GameObject EnableObject()
    {
        GameObject objResult = null;

        objResult = base.EnableObject();

        StartCoroutine(ReleaseEffect(objResult));

        return objResult;
    }

    private IEnumerator ReleaseEffect(GameObject objTarget)
    {
        yield return new WaitForSeconds(1.0f);

        ReleaseObject(objTarget);
    }
}
