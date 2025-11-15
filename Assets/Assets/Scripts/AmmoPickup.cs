using UnityEngine;

public class AmmoPickup : Item
{
    [SerializeField] float m_rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);
    }
    

    public override bool OnPickup(Collider other)
    {
        //! Allow player reload full by down any key
        other.gameObject.GetComponentInChildren<Backpack>()?.ResetCapacityAmmorSizeForAllWeapons();

        return true;
    }

}
