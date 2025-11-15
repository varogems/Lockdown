using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem m_particle;


    public void Explore()
    {

        // Explosion and damage arround to everything
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Config.m_EnemyRadiusExplosion);
        
        foreach (var hitCollider in hitColliders)
        {
            //! Damage all in curr area
        }

        // Destroy(this.gameObject);

    }
}
