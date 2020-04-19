using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public float       m_fIntervalTime  = 0.5f;
    public Transform[] m_arraySpawnPool = null;

    private void Start()
    {
        InvokeRepeating("Spawn", m_fIntervalTime, m_fIntervalTime);

        Debug.Log("Call_Enemy_Spawner");
    }

    private void Spawn()
    {
        if (!SlimePool.GetInstance())
            return;

        int        nSpawnPoolIndex = 0;
        GameObject objEnemy        = null;

        objEnemy = SlimePool.GetInstance().EnableObject();

        if (!objEnemy)
            return;

        nSpawnPoolIndex = Random.Range(0, m_arraySpawnPool.Length);

        objEnemy.transform.position = m_arraySpawnPool[nSpawnPoolIndex].position;
        objEnemy.transform.rotation = m_arraySpawnPool[nSpawnPoolIndex].rotation;

        SlimePool.GetInstance().Setup(objEnemy);
    }
}
