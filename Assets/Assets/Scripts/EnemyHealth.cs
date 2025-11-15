using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : Health
{
    [SerializeField] DetectRange m_detectRange;
    [SerializeField] Animator m_animator = null;
    [SerializeField] Collider m_collider;
    [SerializeField] float m_delayToDeath = .5f;
    [SerializeField] bool m_isExplosion = false;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_deathSFX;

    Gate m_gate;


    void Die()
    {
        if (m_detectRange.IsActive())
            StartCoroutine(IEDie());
    }

    IEnumerator IEDie()
    {
        m_collider.enabled = false;

        m_detectRange?.Disable();

        if (m_animator != null)
            m_animator.Play("Die", 0, 0f);

        // m_animator?.SetBool("IsDie", true);

        if (m_audioSource != null)
            m_audioSource.PlayOneShot(m_deathSFX);

        yield return new WaitForSeconds(m_delayToDeath);

        if(m_isExplosion)
            PoolManager.ExplosionEffect(this.transform.position);

        m_gate?.Reduce();

        this.gameObject.SetActive(false);
        
    }


    public override void TakeDame(int damage)
    {
        base.TakeDame(damage);
        
        if (m_curHealth < 1)
            Die();
    }

    public override void Reset()
    {
        base.Reset();
        m_collider.enabled = true;
        // m_animator.Play("Idle", 0, 0f);

    }

    public void SetGate(Gate gate)
    {
        m_gate = gate;
    }
    
}
