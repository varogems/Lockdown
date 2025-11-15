using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int m_maxHealth = 100;
    protected int m_curHealth;

    protected virtual void Awake()
    {
        m_curHealth = m_maxHealth;
    }

    public virtual void TakeDame(int damage)
    {
        m_curHealth -= damage;
    }

    public virtual void Reset()
    {
        m_curHealth = m_maxHealth;
    }


} 
