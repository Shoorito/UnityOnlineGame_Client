using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : ObjectPool
{
    private static DamageTextPool m_refInstance = null;

    public static DamageTextPool GetInstance()
    {
        if(!m_refInstance)
        {
            Debug.LogWarning("'DamageTextPool'을 'Create' 함수로 생성하시고 사용해야 합니다.");

            return null;
        }

        return m_refInstance;
    }

    public static DamageTextPool Create
    (
        string strPrefabName,    // "해당 'Pool'에서 관리 할 프리팹의 이름"
        string strObjectName,    // "'PoolObject'의 이름"
        int    nPoolItemMaxCount // "'Pool'에서 관리 할 오브젝트의 최고 개수"
    )
    {
        GameObject objContainer = null;

        objContainer  = new GameObject();
        m_refInstance = objContainer.AddComponent(typeof(DamageTextPool)) as DamageTextPool;

        m_refInstance.m_strPrefabName   = strPrefabName;
        m_refInstance.m_nObjectMaxCount = nPoolItemMaxCount;
        m_refInstance.gameObject.name   = strObjectName;

        m_refInstance.PreloadObject();

        return m_refInstance;
    }
}
