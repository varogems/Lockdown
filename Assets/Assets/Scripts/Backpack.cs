using Cinemachine;
using TMPro;
using UnityEngine;

public class Backpack : MonoBehaviour
{

    [SerializeField] StarterAssets.StarterAssetsInputs m_starterAssetsInputs;
    [SerializeField] GameObject m_scopeGameObject;
    [SerializeField] GameObject m_playerCameraRoot;
    [SerializeField] GameObject[] m_listCurWeapon;
    [SerializeField] Transform m_rootTransformPlayer;
    [SerializeField] float  m_distanceDrop = 2f;
    [SerializeField] GameObject m_crosshair;
    [SerializeField] CinemachineVirtualCamera m_cvc;
    [SerializeField] Camera m_weaponCamera;
    [SerializeField] TextMeshProUGUI m_ammorTxt;

    GameObject m_curActiveWeapon;

    void Awake()
    {
        InitBackpack();
    }

    void InitBackpack()
    {
        m_listCurWeapon = new GameObject[3];

        m_listCurWeapon[0] = null;
        m_listCurWeapon[1] = null;
        m_listCurWeapon[2] = null;

    }

    public GameObject GetPlayerCameraRoot()
    {
        return m_playerCameraRoot;
    }


     public bool PickupWeapon(WeaponPickup weaponPickup)
    {
        InfoWeaponSO infoWeaponSO = weaponPickup.getInfoWeaponSO();

        if (infoWeaponSO.m_typeWeapon == InfoWeaponSO.TypeWeapon.Weapon_1 && m_listCurWeapon[0])
            return false;

        if (infoWeaponSO.m_typeWeapon == InfoWeaponSO.TypeWeapon.Weapon_2 && m_listCurWeapon[1])
            return false;

        if (infoWeaponSO.m_typeWeapon == InfoWeaponSO.TypeWeapon.Weapon_3 && m_listCurWeapon[2])
            return false;


        // GameObject weapon = Instantiate(infoWeaponSO.m_modelActiveWeapon, transform);
        
        m_starterAssetsInputs.isZoom = false;

        GameObject weaponGameObject = Instantiate(infoWeaponSO.m_modelActiveWeapon);
        weaponGameObject.SetActive(false);

        //! If player hasn't weapon, we show weapon ingame.
        if (m_curActiveWeapon == null)
        {
            weaponGameObject.transform.SetParent(this.transform, false);
            weaponGameObject.SetActive(true);

            m_curActiveWeapon = weaponGameObject;

            //! Hide crosshair if snip gun
            if (infoWeaponSO.m_typeWeapon == InfoWeaponSO.TypeWeapon.Weapon_2)
            {
                m_crosshair.SetActive(false);
                weaponGameObject.GetComponent<Weapon>()?.SetScope(m_cvc, m_weaponCamera, m_scopeGameObject);

                // Debug.Log("Sniper");
            }
            else
            {
                m_crosshair.SetActive(true);            
                m_cvc.m_Lens.FieldOfView = infoWeaponSO.m_zoomOutAmmount;
                m_weaponCamera.fieldOfView = infoWeaponSO.m_zoomOutAmmount;
            }



        }


        m_listCurWeapon[(int)infoWeaponSO.m_typeWeapon] = weaponGameObject;

   

        //! SetStarterAssetsInputs for weapon.
        Weapon scriptWeapon = weaponGameObject.GetComponent<Weapon>();
        scriptWeapon.SetCurAmmorSize(weaponPickup.GetCurAmmorSize());
        scriptWeapon.SetStarterAssetsInputs(m_starterAssetsInputs);
        scriptWeapon.SetInfoWeapon(infoWeaponSO);

        if (infoWeaponSO.m_typeWeapon == InfoWeaponSO.TypeWeapon.Weapon_2)
        {
            weaponGameObject.GetComponent<Weapon>()?.SetScope(m_cvc, m_weaponCamera, m_scopeGameObject);
            
            // Debug.Log("Sniper");
        }



        m_curActiveWeapon.GetComponent<Weapon>().SetAmmorTMT(m_ammorTxt);


        weaponGameObject = null;
        scriptWeapon = null;

        return true;
    }



    public void SwapWeapon()
    {

        bool isSelectedWeapon1 = m_starterAssetsInputs.isSelectedWeapon1;
        bool isSelectedWeapon2 = m_starterAssetsInputs.isSelectedWeapon2;
        bool isSelectedWeapon3 = m_starterAssetsInputs.isSelectedWeapon3;

        if (!isSelectedWeapon1 && !isSelectedWeapon2 && !isSelectedWeapon3)
            return;

        if (isSelectedWeapon1 && m_curActiveWeapon != m_listCurWeapon[0] && m_listCurWeapon[0])
        {
            ActiveWeaponInBackpack(ref m_listCurWeapon[0]);
            return;
        }

        if (isSelectedWeapon2 && m_curActiveWeapon != m_listCurWeapon[1] && m_listCurWeapon[1])
        {
            ActiveWeaponInBackpack(ref m_listCurWeapon[1]);

            //! Reset crosshair & scope for Snip
            m_crosshair.SetActive(false);

            return;
        }

        if (isSelectedWeapon3 && m_curActiveWeapon != m_listCurWeapon[2] && m_listCurWeapon[2])
        {
            ActiveWeaponInBackpack(ref m_listCurWeapon[2]);
            return;
        }

    }

    void ActiveWeaponInBackpack(ref GameObject selectedWeapon)
    {
        //! Disable previour weapon
        m_curActiveWeapon?.SetActive(false);
        m_curActiveWeapon?.transform.SetParent(null, false);

        //! Active selected weapon
        m_curActiveWeapon = selectedWeapon;
        m_curActiveWeapon.SetActive(true);
        m_curActiveWeapon.transform.SetParent(this.transform, false);

        m_starterAssetsInputs.isZoom = false;

        //! Reset crosshair & scope
        m_crosshair.SetActive(true);
        m_scopeGameObject.SetActive(false);

        //! Reset field of view
        InfoWeaponSO infoWeaponSO = m_curActiveWeapon.GetComponent<Weapon>()?.GetInfoWeapon();
        m_cvc.m_Lens.FieldOfView = infoWeaponSO.m_zoomOutAmmount;
        m_weaponCamera.fieldOfView = infoWeaponSO.m_zoomOutAmmount;

        
        m_curActiveWeapon.GetComponent<Weapon>().SetAmmorTMT(m_ammorTxt);

        // m_curActiveWeapon.GetComponent<Weapon>()?.LogAmmorSize();
    }



    void Drop()
    {
        if (m_starterAssetsInputs.isDrop && m_curActiveWeapon)
        {
            //! Remove it out backpack
            for (int i = 0; i < m_listCurWeapon.Length; i++)
                if (m_listCurWeapon[i] == m_curActiveWeapon)
                    m_listCurWeapon[i] = null;


            //! Disable cur weapon & destroy ActiveWeapon gameobject
            m_curActiveWeapon.SetActive(false);
            m_curActiveWeapon.transform.SetParent(null);


            //! Assign current info weapon to PickupWeapon gameobject.
            Weapon weaponScript = m_curActiveWeapon.GetComponent<Weapon>();

            GameObject pickupWeaponGameObject = Instantiate(weaponScript.GetInfoWeapon().m_modelPickupWeapon);
            pickupWeaponGameObject.GetComponent<WeaponPickup>()?.SetCurAmmorSize(weaponScript.GetCurAmmorSize());

            weaponScript = null;

            //! Release memory weapon.
            Destroy(m_curActiveWeapon);
            m_curActiveWeapon = null;




            //! Drop weapon in world.
            Vector3 distanceDrop = Camera.main.transform.forward.normalized * m_distanceDrop;
            pickupWeaponGameObject.transform.position = new Vector3(Camera.main.transform.position.x + distanceDrop.x,
                                                        m_rootTransformPlayer.position.y + 0.3f,
                                                        Camera.main.transform.position.z + distanceDrop.z);
            pickupWeaponGameObject = null;



            m_crosshair.SetActive(true);
            m_scopeGameObject.SetActive(false);

            m_starterAssetsInputs.isZoom = false;

            m_ammorTxt.text = "";

        }

    }

    public void ResetCapacityAmmorSizeForAllWeapons()
    {
        for (int i = 0; i < m_listCurWeapon.Length; i++)
        {
            if (m_listCurWeapon[i] != null)
                m_listCurWeapon[i].GetComponent<Weapon>().ResetCapacityAmmorSize();
        }
    }
    

    void Update()
    {
        SwapWeapon();
        Drop();
    }


}
