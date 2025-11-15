using UnityEngine;

[CreateAssetMenu(fileName = "InfoWeaponSO", menuName = "Scriptable Objects/InfoWeaponSO")]
public class InfoWeaponSO : ScriptableObject
{
    public enum TypeWeapon
    {
        Weapon_1 = 0, // Rifle
        Weapon_2, // Sniper
        Weapon_3, // Pistol
    }

    public GameObject m_modelActiveWeapon;
    public GameObject m_modelPickupWeapon;
    public TypeWeapon m_typeWeapon;

    public int m_damage                 = 50;
    public float m_fireRate             = 1.0f;
    public int m_defaultMagazineSize    = 13;
    public int m_defaultCapacity        = 52;
    
    public bool m_isAutomatic           = false;
    public bool m_canZoom               = false;
    public float m_weight               = 1.5f;
    public bool m_canInspect            = false;
    public float m_zoomInAmmount        = 11f;
    public float m_zoomOutAmmount = 60f;
    public AudioClip m_shootSFX;
    public AudioClip m_reloadSFX;
    public float m_delayReload = 0.3f;

   

}
