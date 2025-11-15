using UnityEngine;

public abstract class LongRangeEnemy : MovingEnemy
{
    [SerializeField] protected GameObject m_muzzle;
    [SerializeField] protected ShootingMuzzle m_shootingMuzzleScript;
    [SerializeField] protected PoolManager.BulletType m_bulletType;



    protected override void StopAttack()
    {
        base.StopAttack();

        m_shootingMuzzleScript?.SetActive(false);
    }
    

    protected override void Attack()
    {
        base.Attack();

        SpawnProjectile();

    }


    public virtual void SpawnProjectile()
    {

        m_shootingMuzzleScript?.SetDestination(m_muzzle, m_followTarget, m_bulletType);
        
        m_shootingMuzzleScript?.SetActive();
    }
}
