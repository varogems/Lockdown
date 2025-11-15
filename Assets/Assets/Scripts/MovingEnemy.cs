using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovingEnemy : Enemy
{
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] protected Animator m_animator;


    protected void Idle()
    {
        AnimIdle();
    }

    protected void Chase()
    {
        m_agent.SetDestination(m_followTarget.transform.position);
        AnimMoving();
    }

    public override void Reset()
    {
        base.Reset();

        StartCoroutine(IEResetAgent());
    }

    IEnumerator IEResetAgent()
    {
        m_agent.enabled = false;
        yield return new WaitForSeconds(Mathf.Epsilon);
        m_agent.enabled = true;
        m_agent.ResetPath();
    }


    public override void Active(bool isActive = true)
    {
        base.Active(isActive);

        if (!m_isActive)
        {
            Idle();
            StopAttack();
            m_agent.ResetPath();
        }


    }    
    
    protected virtual void Attack()
    {
        m_lookatTargetScript.SetActive();
        AnimAttack();
    }

    protected virtual void StopAttack()
    {
        m_lookatTargetScript.SetActive(false);
    }


    void FixedUpdate()
    {
        if (!m_isActive)
        {
            Idle();
            return;
        }

        if(m_followTarget == null) return;

        Chase();

        if (m_agent.remainingDistance <= m_agent.stoppingDistance && m_agent.remainingDistance != 0)
            Attack();
        else
            StopAttack();
    }


    public abstract void AnimIdle();
    public abstract void AnimAttack();

    public abstract void AnimMoving();

    public abstract void AnimDie();
    

}
