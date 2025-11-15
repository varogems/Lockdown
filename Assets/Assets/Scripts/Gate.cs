using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] float m_timeSpawn = 3f;
    [SerializeField] int m_capacity = 5;
    [SerializeField] int m_numberOfEnemy;
    [SerializeField] PoolManager.EnemyType m_typeEnemy;
    bool m_isActive;
    
    void Start()
    {
        m_isActive = true;
        m_numberOfEnemy = 0;

        StartCoroutine(IESpawn());

    }

    IEnumerator IESpawn()
    {
        while (m_isActive)
        {
            yield return new WaitForSeconds(m_timeSpawn);

            if (m_numberOfEnemy > (m_capacity - 1)) continue;

            PoolManager.SpawnEnemy(m_typeEnemy, this.transform.position, this);
            m_numberOfEnemy++;

        }
    }

    public void Reduce()
    {
        m_numberOfEnemy--;
    }
}
