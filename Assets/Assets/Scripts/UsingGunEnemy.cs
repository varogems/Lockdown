using UnityEngine;

public class UsingGunEnemy : LongRangeEnemy
{
    public override void AnimAttack()
    {
        m_animator.SetBool("IsMoving", false);
        m_animator.SetBool("IsShooting", true);
    }

    public override void AnimDie()
    {
    }

    public override void AnimIdle()
    {
        m_animator.SetBool("IsMoving", false);
        m_animator.SetBool("IsShooting", false);
    }

    public override void AnimMoving()
    {
        m_animator.SetBool("IsMoving", true);
        m_animator.SetBool("IsShooting", false);
    }


}
