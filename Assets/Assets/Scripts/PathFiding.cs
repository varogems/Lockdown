using UnityEngine;
using UnityEngine.AI;

public class PathFiding : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] GameObject m_followTarget;

    public bool m_isEnable;

    void Awake()
    {
        m_isEnable = false;
    }

    void Update()
    {
        if (!m_isEnable) return;
        
        m_agent.SetDestination(m_followTarget.transform.position);
    }
}
