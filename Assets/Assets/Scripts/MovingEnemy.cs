using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovingEnemy : Enemy
{
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected bool m_isWalkArround = false;
    Coroutine m_crtDelayIdle = null;
    bool m_isDead  = false;

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


        m_isWalkArround = false;
        m_isDead = false;

        if(m_crtDelayIdle != null)
        {
            StopCoroutine(m_crtDelayIdle);
            m_crtDelayIdle = null;
        }

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
        else
        {
            m_isWalkArround = false;

            if(m_crtDelayIdle != null)
            {
                StopCoroutine(m_crtDelayIdle);
                m_crtDelayIdle = null;
            }
        }


    }
    public override void Dead()
    {
        m_isDead = true;
        m_agent.ResetPath();
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


    void WalkArround()
    {
        if(m_agent.enabled == false) return;

        if(!m_isWalkArround)
        {
            if(m_crtDelayIdle != null)
            {
                StopCoroutine(m_crtDelayIdle);
                m_crtDelayIdle = null;
            }


            Vector3 posDestination = Config.RamdomRelativePos();
            posDestination += this.transform.position;
            posDestination.y -= 1;

            NavMeshHit navMeshHit;
            if(m_agent.Raycast(posDestination, out navMeshHit))
                m_agent.SetDestination(navMeshHit.position);
            else
                m_agent.SetDestination(posDestination);

            AnimMoving();
            m_isWalkArround = true;
            return;
        }
        if ((m_crtDelayIdle == null) && m_isWalkArround && 
            m_agent.remainingDistance <= m_agent.stoppingDistance && m_agent.remainingDistance != 0)
        {
            m_crtDelayIdle = StartCoroutine(DelayIdle());
            return;
        }

    }

    IEnumerator DelayIdle()
    {
        AnimIdle();
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 3));
        m_isWalkArround = false;
    }


    void FixedUpdate()
    {
        if(m_isDead) return;

        if (!m_isActive)
        {
            // Idle();
            WalkArround();
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
