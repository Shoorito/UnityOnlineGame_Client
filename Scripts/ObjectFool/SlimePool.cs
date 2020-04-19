using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool
{
    private static SlimePool m_refInstance = null;

    public static SlimePool GetInstance()
    {
        if (!m_refInstance)
        {
            Debug.Log("'Create' 함수를 이용해 먼저 'Pool' 클래스를 생성하십시오.");

            return null;
        }

        return m_refInstance;
    }

    public static SlimePool Create
    (
        string strPrefabName, // "해당 'Pool'에서 관리 할 프리팹의 이름"
        string strObjectName, // "'PoolObject'의 이름"
        int nPoolItemMaxCount // "'Pool'에서 관리 할 오브젝트의 최고 개수"
    )
    {
        GameObject objContainer = null;

        objContainer  = new GameObject();
        m_refInstance = objContainer.AddComponent(typeof(SlimePool)) as SlimePool;

        m_refInstance.m_strPrefabName   = strPrefabName;
        m_refInstance.m_nObjectMaxCount = nPoolItemMaxCount;
        m_refInstance.gameObject.name   = strObjectName;

        m_refInstance.PreloadObject();

        return m_refInstance;
    }

    public override GameObject EnableObject()
    {
        GameObject objTarget = null;

        if (m_arrObjectPool[(int)E_STATE.E_SLEEP].Count > 0)
        {
            objTarget = m_arrObjectPool[(int)E_STATE.E_SLEEP][0];

            m_arrObjectPool[(int)E_STATE.E_SLEEP].RemoveAt(0);
            m_arrObjectPool[(int)E_STATE.E_USED].Add(objTarget);

            objTarget.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning("선택한 타입의 리스트의 맴버를 모두 사용 중입니다!");
        }

        return objTarget;
    }

    public void Setup(GameObject objTarget)
    {
        EnemyHealth enemyHealth = null;

        enemyHealth = objTarget.GetComponent<EnemyHealth>();

        enemyHealth.Init();
        objTarget.SetActive(true);
    }
}
