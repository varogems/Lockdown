using System.Collections;
using UnityEngine;

public class ShootingMuzzle : MonoBehaviour
{
    
    [Header("Not assign")]
    [SerializeField] float m_fireRate;
    [SerializeField] int m_damage;
    [SerializeField] GameObject m_targetObj;
    [SerializeField] GameObject m_muzzle;
    [SerializeField] PoolManager.BulletType m_bulletType;
    [SerializeField] bool m_isActive;
    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_audioClip;


    Coroutine m_crtShoot;

    void Awake()
    {
        m_isActive = false;
        m_crtShoot = null;
    }

    public void SetDestination(GameObject muzzleObj, GameObject targetObj,
                                PoolManager.BulletType bulletType)
    {
        m_muzzle        = muzzleObj;
        m_targetObj     = targetObj;
        m_bulletType    = bulletType;
    }



    public void SetActive(bool isActive = true) => m_isActive = isActive;

    void Update()
    {

        if (!m_isActive)
        {
            if (m_crtShoot != null)
            {
                StopCoroutine(m_crtShoot);
                m_crtShoot = null;
            }
            return;
        }

        if (m_crtShoot == null)
            m_crtShoot = StartCoroutine(Fire());

    }




    IEnumerator Fire()
    {
        while (true)
        {
            PoolManager.SpawnBulletEnemy(m_bulletType, this.gameObject.layer, m_targetObj, m_muzzle);

            //! Need Sound for shoot action
            m_audioSource.PlayOneShot(m_audioClip);

            yield return new WaitForSeconds(m_fireRate);
        }

    }

    public void Reset()
    {
        m_fireRate = 0.5f;
        m_damage = 20;
        m_targetObj = null;
        m_muzzle = null;
        m_bulletType = PoolManager.BulletType.None;
        m_isActive = false;

        if (m_crtShoot != null)
        {
            StopCoroutine(m_crtShoot);
            m_crtShoot = null;
        }
    }
}
