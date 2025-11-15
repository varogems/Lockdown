using UnityEngine;

public class Robot : LongRangeEnemy
{

    public override void AnimIdle()
    {
        m_animator.SetBool("IsMoving", false);
    }

    public override void AnimAttack()
    {
        m_animator.SetBool("IsMoving", true);
    }

    public override void AnimMoving()
    {
        m_animator.SetBool("IsMoving", true);
    }
    
    public override void AnimDie()
    {
        
    }
}
