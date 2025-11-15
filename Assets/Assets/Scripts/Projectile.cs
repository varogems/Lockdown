using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int m_damage;
    [SerializeField] float m_speed = 15f;
    [SerializeField] float m_lifeTime = 1.5f;
    [SerializeField] Rigidbody m_rb;
    PoolManager.BulletType m_bulletType;
    
    int m_layerOwner;
    GameObject m_owner;

    public void SetInput(int layer, PoolManager.BulletType bulletType,int damage, float speed, float lifeTime)
    {
        m_bulletType = bulletType;
        m_damage = damage;
        m_speed = speed;
        m_lifeTime = lifeTime;

        m_rb.linearVelocity = transform.forward * m_speed;
        m_layerOwner = layer;
        StartCoroutine(ActiveLifetime());
    }

    void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer) == LayerMask.GetMask("Ignore Raycast"))
            return;

        if ((1 << other.gameObject.layer) == LayerMask.GetMask("Enemy"))
            return;

        if (other.gameObject.layer.CompareTo(m_layerOwner) == 0) return;

        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, this.transform.forward,
                            out raycastHit))
        {
            PoolManager.EnemyParticleType enemyParticleType;
            
            switch (m_bulletType)
            {
                case PoolManager.BulletType.BulletTurret:
                    enemyParticleType = PoolManager.EnemyParticleType.TurretHitEffect;
                    break;
                case PoolManager.BulletType.BulletRobot:
                    enemyParticleType = PoolManager.EnemyParticleType.RobotHitEffect;
                    break;
                case PoolManager.BulletType.BulletHuman:
                    enemyParticleType = PoolManager.EnemyParticleType.HumanHitEffect;
                    break;
                default:
                    enemyParticleType = PoolManager.EnemyParticleType.None;
                    break;
            }
            PoolManager.SpawnEnemyHitVfc(enemyParticleType, raycastHit);
        }

        //! If player
        if ((1 << other.gameObject.layer) == LayerMask.GetMask("Player"))
        {
            // Debug.Log("Tag " + other.tag  + " Player take damage " + m_damage);

            other.GetComponent<PlayerHealth>()?.TakeDame(m_damage);
        }

        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    IEnumerator ActiveLifetime()
    {
        yield return new WaitForSeconds(m_lifeTime);
        this.gameObject.SetActive(false);

    }
    

}
