
using System.Collections;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] LayerMask m_layer;
    [SerializeField] Animator m_animator;
    [SerializeField] ParticleSystem m_muzzleFlash;
    [SerializeField] AmmorSize m_ammorSize;

    [SerializeField] AudioSource m_audioSource;

    StarterAssets.StarterAssetsInputs m_starterAssetsInputs;
    InfoWeaponSO m_infoWeapon;
    GameObject m_scope;
    CinemachineVirtualCamera m_cvc = null;
    Camera m_weaponCamera;
    TextMeshProUGUI m_ammorTxt;
    float m_lastTimeShot = 0;


    const string m_strReloadAnim = "Reload";
    const string m_strShootAnim = "Shoot";
    const string m_strIdledAnim = "Idle";

    void Update()
    {
        m_lastTimeShot += Time.deltaTime;

        Reload();
        Zoom();
        Shoot();
    }


    ///--------------------- InfoWeapon ----------------------------------------
    public void SetInfoWeapon(InfoWeaponSO infoWeapon)
    {
        m_infoWeapon = infoWeapon;
    }

    public InfoWeaponSO GetInfoWeapon()
    {
        return m_infoWeapon;
    }


    ///--------------------- AmmorSize ----------------------------------------
    public void SetCurAmmorSize(AmmorSize ammorSize)
    {
        m_ammorSize = ammorSize;
    }

    public AmmorSize GetCurAmmorSize()
    {
        return m_ammorSize;
    }

    public void SetAmmorTMT(TextMeshProUGUI textMeshProUGUI)
    {
        m_ammorTxt = textMeshProUGUI;
        RefreshAmmorTextGUI();
    }

    public void RefreshAmmorTextGUI()
    {
        m_ammorTxt.text = m_ammorSize.m_curMagazineSize.ToString() + "/" + m_ammorSize.m_curCapacity.ToString();

        // Debug.Log("TypeWeapon: " + m_infoWeapon.m_typeWeapon +
        //         " CurAmmorSize: " + m_ammorSize.m_curMagazineSize + "/" + m_ammorSize.m_curCapacity);

    }
    //!------------------------------------------------------------------------
    public void ResetCapacityAmmorSize()
    {
        m_ammorSize.m_curCapacity = m_infoWeapon.m_defaultCapacity;
    }

    ///--------------------- StarterAssetsInputs -------------------------------

    public void SetStarterAssetsInputs(StarterAssets.StarterAssetsInputs sai)
    {
        m_starterAssetsInputs = sai;
    }


    ///--------------------- Scope for snip ----------------------------------------
    public void SetScope(CinemachineVirtualCamera cvc, Camera weaponCamera, GameObject scope, bool isVisible = false)
    {
        m_scope = scope;
        m_scope.SetActive(isVisible);

        m_cvc = cvc;
        m_weaponCamera = weaponCamera;
    }



    void Zoom()
    {
        if (m_scope == null) return;

        if (m_starterAssetsInputs.isZoom)
        {
            m_cvc.m_Lens.FieldOfView = m_infoWeapon.m_zoomInAmmount;
            m_weaponCamera.fieldOfView = m_infoWeapon.m_zoomInAmmount;
        }
        else
        {
            m_cvc.m_Lens.FieldOfView = m_infoWeapon.m_zoomOutAmmount;
            m_weaponCamera.fieldOfView = m_infoWeapon.m_zoomOutAmmount;
        }
        
        m_scope?.SetActive(m_starterAssetsInputs.isZoom);

    }


    void Shoot()
    {
        if (!m_starterAssetsInputs) return;

        if (!m_starterAssetsInputs.isShoot) return;


        //! Need Reload ammor.
        if (m_ammorSize.m_curMagazineSize < 1)
        {
            // Reload();
            return;
        }

        if (m_lastTimeShot > m_infoWeapon.m_fireRate)
        {
            m_lastTimeShot = 0;
            m_ammorSize.m_curMagazineSize--;
            RefreshAmmorTextGUI();
            
            m_animator.Play(m_strShootAnim, 0, 0f);
            m_muzzleFlash.Play();
            m_audioSource.PlayOneShot(m_infoWeapon.m_shootSFX);



            Vector3 deviation = Vector3.zero;

            if (m_scope != null && !m_starterAssetsInputs.isZoom)
                deviation = new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));


            RaycastHit raycastHit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward + deviation,
                                out raycastHit, Mathf.Infinity, m_layer, QueryTriggerInteraction.Ignore))
            {
                PoolManager.SpawnPlayerHitVfc(PoolManager.PlayerParticleType.PlayerHitEffect, raycastHit);

                Collider collider = raycastHit.collider;

                // Debug.Log(collider.name);

                if ((1 << collider.gameObject.layer) == LayerMask.GetMask("Danger"))
                    collider.gameObject.GetComponent<Explosion>()?.Explore();

                collider.gameObject.GetComponent<EnemyHealth>()?.TakeDame(m_infoWeapon.m_damage);

            }

            Camera.main.transform.forward += deviation * 100;
            
            // Debug.Log(Camera.main.transform.forward);

            // //! Error when raycast with sky
            // if (!raycastHit.IsUnityNull())
            //     Debug.Log(raycastHit.collider.name);
        }
        
        if(!m_infoWeapon.m_isAutomatic)
            m_starterAssetsInputs.ShootInput(false);

        

    }

    void Reload()
    {
        if (!m_starterAssetsInputs) return;
        if (!m_starterAssetsInputs.isReload) return;
        if (!CaculateReloadAmmor()) return;

        m_starterAssetsInputs.isZoom = false;
        m_scope?.SetActive(false);

        m_animator.Play(m_strReloadAnim, 0, 0f);
        StartCoroutine(IEDelayReloadSFX());

        m_starterAssetsInputs.ReloadInput(false);
        RefreshAmmorTextGUI();

    }

    IEnumerator IEDelayReloadSFX()
    {
        yield return new WaitForSeconds(m_infoWeapon.m_delayReload);
        m_audioSource.PlayOneShot(m_infoWeapon.m_reloadSFX);
        
    }


    public bool CaculateReloadAmmor()
    {
        if (m_ammorSize.m_curMagazineSize == m_infoWeapon.m_defaultMagazineSize) return false;
        if (m_ammorSize.m_curCapacity < 1) return false;


        int totalAmmorSize = m_ammorSize.m_curMagazineSize + m_ammorSize.m_curCapacity;
        m_ammorSize.m_curMagazineSize   = ((totalAmmorSize / m_infoWeapon.m_defaultMagazineSize) >= 1) ?
                                            m_infoWeapon.m_defaultMagazineSize : totalAmmorSize;

        m_ammorSize.m_curCapacity       = totalAmmorSize - m_ammorSize.m_curMagazineSize;

        RefreshAmmorTextGUI();
        return true;
       
    }


}
