using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public enum E_STATE
    {
        E_SLEEP,
        E_USED,
        E_MAX
    }

    public int    m_nObjectMaxCount = 0;
    public string m_strPrefabName   = "";

    private bool m_isNowUsing  = false;
    private bool m_isPreloaded = false;
    private bool m_isNowReleaseUsing = false;

    protected List<GameObject>[] m_arrObjectPool  = new List<GameObject>[(int)E_STATE.E_MAX];

    // 지정한 개수 만큼의 오브젝트 개체를 미리 생성합니다.
    // 이 함수는 한 번 사용 한 후 다시 사용 할 수 없습니다.
    // 생성된 개체들은 모두 "SLEEP"리스트에 정보가 저장됩니다.
    public void PreloadObject()
    {
        if (m_nObjectMaxCount <= 0)
        {
            Debug.LogError("관리 오브젝트의 최대 개수는 '0' 이하가 될 수 없습니다.");

            return;
        }

        if (m_isPreloaded)
        {
            Debug.Log("\"Preload\"기능은 생성 후 단 한 번만 사용 할 수 있습니다.");

            return;
        }

        m_isPreloaded   = true;

        m_arrObjectPool[(int)E_STATE.E_SLEEP] = new List<GameObject>();
        m_arrObjectPool[(int)E_STATE.E_USED]  = new List<GameObject>();

        for (int nObject = 0; nObject < m_nObjectMaxCount; nObject++)
        {
            GameObject objTemp = null;

            objTemp = Instantiate(Resources.Load(m_strPrefabName) as GameObject);

            objTemp.transform.SetParent(transform);
            objTemp.SetActive(false);

            m_arrObjectPool[(int)E_STATE.E_SLEEP].Add(objTemp);
        }
    }

    // 지정한 타입의 리스트의 맨 앞에 위치한 적 개체를 가져옵니다.
    // 적 개체의 타입은 지정 타입의 반대로 변하고, 반대 리스트에 추가됩니다.
    // 클래스 생성 시 지정한 크기 이상의 적 개체는 생성 할 수 없습니다.
    // 한 번 리스트에서 꺼낼 때마다 부모 노드는 "ObjectPool"을 가지고 있는 노드로 고정됩니다.
    public virtual GameObject EnableObject()
    {
        if(m_isNowUsing)
        {
            Debug.Log("현재, 먼저 호출 된 명령이 처리 중 입니다..");
            Debug.Log("잠시 후 다시 시도 하세요.");

            return null;
        }

        GameObject objTarget = null;

        m_isNowUsing = true;

        if(m_arrObjectPool[(int)E_STATE.E_SLEEP].Count > 0)
        {
            objTarget = m_arrObjectPool[(int)E_STATE.E_SLEEP][0];

            m_arrObjectPool[(int)E_STATE.E_SLEEP].RemoveAt(0);
            m_arrObjectPool[(int)E_STATE.E_USED].Add(objTarget);

            objTarget.SetActive(true);
            objTarget.transform.SetParent(transform);
        }
        else
        {
            Debug.LogWarning("선택한 타입의 리스트의 맴버를 모두 사용 중입니다!");
        }

        m_isNowUsing = false;

        return objTarget;
    }

    public void ReleaseObject(GameObject objTarget)
    {
        if(m_isNowReleaseUsing)
        {
            Debug.Log("현재, 먼저 호출 된 'Release' 명령이 처리 중 입니다..");
            Debug.Log("잠시 후 다시 시도 하세요.");

            return;
        }

        m_isNowReleaseUsing = true;

        m_arrObjectPool[(int)E_STATE.E_USED].Remove(objTarget);
        m_arrObjectPool[(int)E_STATE.E_SLEEP].Add(objTarget);

        objTarget.SetActive(false);
        objTarget.transform.SetParent(transform);

        m_isNowReleaseUsing = false;
    }
}
