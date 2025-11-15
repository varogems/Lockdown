using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    protected const string m_TagPlayer = "Player";
    
    void OnTriggerEnter(Collider other)
    {
        // if (!other.CompareTag(m_TagPlayer))
        //     return;

        if ((1 << other.gameObject.layer) != LayerMask.GetMask("Player"))
            return;

        if(OnPickup(other))
            Destroy(this.gameObject);
    }

    abstract public bool OnPickup(Collider other);
}
