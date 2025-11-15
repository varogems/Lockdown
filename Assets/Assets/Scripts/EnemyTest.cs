using UnityEngine;
using UnityEngine.AI;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_agent;
    GameObject m_target;

    public void SetTarger(GameObject target)
    {
        m_target = target;
    }


    void FixedUpdate()
    {
        if(m_target == null) return;
        if(m_agent== null)return;
        m_agent.SetDestination(m_target.transform.position);
    }


}
