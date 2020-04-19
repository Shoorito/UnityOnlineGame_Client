using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankCellPool : ObjectPool
{
    private static RankCellPool m_refInstance = null;

    public static RankCellPool Create
    (
        string strPrefabName,     // "해당 'Pool'에서 관리 할 프리팹의 이름"
        string strObjectName,     // "'PoolObject'의 이름"
        int    nPoolItemMaxCount, // "'Pool'에서 관리 할 오브젝트의 최고 개수"
        int    nLayerLevel = 1    // 해당 풀의 레이어 계층 레벨을 정합니다.
    )
    {
        GameObject objContainer = null;

        objContainer  = new GameObject();
        m_refInstance = objContainer.AddComponent(typeof(RankCellPool)) as RankCellPool;

        m_refInstance.m_strPrefabName   = strPrefabName;
        m_refInstance.m_nObjectMaxCount = nPoolItemMaxCount;
        m_refInstance.gameObject.name   = strObjectName;
        m_refInstance.gameObject.layer  = nLayerLevel;

        m_refInstance.PreloadObject();

        return m_refInstance;
    }

    public static RankCellPool GetInstance()
    {
        return m_refInstance;
    }
}
