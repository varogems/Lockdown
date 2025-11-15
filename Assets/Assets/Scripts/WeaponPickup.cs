using UnityEngine;

public struct AmmorSize
{
    public int m_curMagazineSize;
    public int m_curCapacity;


    public void InitAmmorFromInfoWeaponSO(InfoWeaponSO infoWeaponSO)
    {
        m_curMagazineSize = infoWeaponSO.m_defaultMagazineSize;
        m_curCapacity = infoWeaponSO.m_defaultCapacity;
    }

    public void SetAmmorSize(int magazineSize, int capacity)
    {
        m_curMagazineSize = magazineSize;
        m_curCapacity = capacity;
    }

    public void SetAmmorSize(AmmorSize ammorSize)
    {
        m_curMagazineSize = ammorSize.m_curMagazineSize;
        m_curCapacity = ammorSize.m_curCapacity;
    }

};



public class WeaponPickup : Pickup
{

    [SerializeField] InfoWeaponSO m_infoWeaponSO;
    [SerializeField] float m_rotationSpeed = 100f;

    [SerializeField] AmmorSize m_curAmmorSize;

    void Awake()
    {
        //! Get ammorsize from infoweapon scriptable object.
        m_curAmmorSize.InitAmmorFromInfoWeaponSO(m_infoWeaponSO);
    }

    void Update()
    {
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);
    }

    public override bool OnPickup(Collider other)
    {
        if (other.GetComponentInChildren<Backpack>().PickupWeapon(this))
            return true;

        return false;
    }

    //-------------------- AmmorSize ----------------------------
    public void SetCurAmmorSize(AmmorSize ammorSize)
    {
        m_curAmmorSize.SetAmmorSize(ammorSize);
    }

    public AmmorSize GetCurAmmorSize()
    {
        return m_curAmmorSize;
    }

    //-------------------- InfoWeaponSO ----------------------------
    public InfoWeaponSO getInfoWeaponSO()
    {
        return m_infoWeaponSO;
    }



}
