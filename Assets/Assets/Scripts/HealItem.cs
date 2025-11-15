using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class HealItem : Item
{
    [SerializeField] float m_rotationSpeed = 100f;

    void Update()
    {
        transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);
    }
    
    public override bool OnPickup(Collider other)
    {
        other.GetComponentInChildren<PlayerHealth>()?.Recovery(Random.Range(2,5) * 10);
        return true;
    }
}
