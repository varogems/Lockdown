using System.Collections;
using UnityEngine;

public class FixedEnemy : Enemy
{
    

    [SerializeField] protected GameObject m_rotateTarget;
    [SerializeField] GameObject m_muzzle;
    [SerializeField] PoolManager.BulletType m_bulletType;    
    [SerializeField] ShootingMuzzle m_shootingMuzzle;
    [SerializeField] bool m_UseRaycast;

    public override void Active(bool isActive = true)
    {
        base.Active(isActive);

        m_lookatTargetScript.SetActive(isActive);
    }


    void FixedUpdate()
    {
        if (!m_isActive)
        {
            DeactiveShoot();
            return;
        }

        if(m_followTarget == null) return;

        Vector3 directionToTarget = m_followTarget.transform.position - m_rotateTarget.transform.position;

        float angleBetween = Vector3.Angle(m_rotateTarget.transform.forward, directionToTarget);


        if (Mathf.Abs(angleBetween) < 10)
            HandleShoot();
        else
            m_shootingMuzzle.SetActive(false);
        
    }


    void HandleShoot()
    {
        if (m_UseRaycast)
        {
            //! One logic error with layermask
            RaycastHit raycastHit;
            if (Physics.Raycast(m_rotateTarget.transform.position, m_rotateTarget.transform.forward,
                out raycastHit, Mathf.Infinity))
            {
                if ((1 << raycastHit.collider.gameObject.layer) == LayerMask.GetMask("Player"))
                    Shoot();
            }
        }
        else Shoot();


    }

    void Shoot()
    {
        m_shootingMuzzle.SetDestination(m_muzzle, m_followTarget, m_bulletType);
        m_shootingMuzzle.SetActive();
    }

    void DeactiveShoot()
    {
        m_shootingMuzzle.SetActive(false);
    }

}
