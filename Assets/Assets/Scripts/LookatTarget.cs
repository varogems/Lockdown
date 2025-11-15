using UnityEngine;

public class LookatTarget : MonoBehaviour
{

    [SerializeField] GameObject m_rotateTarget;
    [SerializeField] float m_speedRotate = 5f;
    public bool m_isActive;
    [SerializeField] GameObject m_followTarget;



    public void SetTarget(GameObject target)
    {
        m_followTarget = target;
    }

    public void SetActive(bool active = true)
    {
        m_isActive = active;
    }

    public void Reset()
    {
        m_isActive = false;
        m_followTarget = null;
    }    



    void FixedUpdate()
    {
        if (m_isActive == false) return;
        if(m_followTarget == null) return;
        
        //! func Unity
        // m_rotateTarget.transform.LookAt(m_followTarget.transform, Vector3.up);


        Vector3 directionToTarget = m_followTarget.transform.position - m_rotateTarget.transform.position;

        // Get the target rotation using Quaternion.LookRotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly interpolate between the current rotation and the target rotation
        m_rotateTarget.transform.rotation = Quaternion.Slerp(m_rotateTarget.transform.rotation, targetRotation, m_speedRotate * Time.fixedDeltaTime);

    }


}
