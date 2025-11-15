using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Config.EnemyType m_enemyType;
    [SerializeField] protected GameObject m_followTarget;
    [SerializeField] protected LookatTarget m_lookatTargetScript;
    [SerializeField] protected bool m_isActive;


    public virtual void Active(bool isActive = true)
    {
        m_isActive = isActive;
    }


    public void SetTarget(GameObject target)
    {
        m_followTarget = target;
        m_lookatTargetScript.SetTarget(target);
    }

    public virtual void Die() { }

    public virtual void Reset()
    {
        m_followTarget = null;
    }

}
