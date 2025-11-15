using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class DetectRange : MonoBehaviour
{
    [SerializeField] Enemy m_enemyScript;
    [SerializeField] NavMeshAgent m_agent;
    [SerializeField] GameObject m_target;
    [SerializeField] GameObject m_exclamationMark;

    [Header("Moving Enemy")]
    [SerializeField] bool m_isLockTarget = false;
    [SerializeField] bool m_isActive = true;

    // string m_TagPlayer = Config.m_tagPlayer;
    string m_TagPlayer = "Player";



    void OnTriggerEnter(Collider other)
    {
        if (!m_isActive) return;

        if (other.CompareTag(m_TagPlayer))
        {

            m_target = other.gameObject.GetComponentInChildren<Backpack>()?.GetPlayerCameraRoot();

            // If target is not Player.
            if (m_target == null) return;

            //! If is moving enemy.
            if (m_agent != null) return;

            // //! For Fixed Enemy
            m_exclamationMark?.SetActive(true);
            m_enemyScript.SetTarget(m_target);
            m_enemyScript.Active();
            //!-------------------------------------------------
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!m_isActive) return;
        
        if (other.CompareTag(m_TagPlayer))
            Reset();
    }


    //! Using for Moving Enemy
    void FixedUpdate()
    {
        //! If not moving enemy.
        if (m_agent == null) return;

        //! If not detect player.
        if (m_target == null) return;

        if (m_isLockTarget) return;

        //! Check Player in view range of moving enemy.
        Vector3 directionToTarget = m_target.transform.position - this.transform.position;
        float angleBetween = Vector3.Angle(this.transform.forward, directionToTarget);
        if (Mathf.Abs(angleBetween) > Config.m_EnemyViewAngle) return;


        //! Make sure no obstacle between Player and Moving Enemy.
        //! Mising case when obstacle is same with this object.
        NavMeshHit hit;
        // m_agent.SetDestination(m_target.transform.position);
        bool isHasObstacle = m_agent.Raycast(m_target.transform.position, out hit);
        // m_agent.ResetPath();
        if (!isHasObstacle)
        {
            m_exclamationMark?.SetActive(true);
            m_enemyScript.SetTarget(m_target);
            m_enemyScript.Active();
            m_isLockTarget = true;

            // Debug.Log("DetectRange m_isLockTarget: " + m_isLockTarget);
            return;
        }

    }

    public void Reset()
    {
        m_isActive = true;

        m_target = null;
        m_exclamationMark?.SetActive(false);
        m_enemyScript.SetTarget(null);
        m_enemyScript.Active(false);
        m_isLockTarget = false;


        // Debug.Log("DetectRange m_isLockTarget: " + m_isLockTarget);
    }

    public void Disable()
    {
        m_isActive = false;

        m_target = null;
        m_exclamationMark?.SetActive(false);
        m_enemyScript.SetTarget(null);
        m_enemyScript.Active(false);
        m_isLockTarget = false;
    }

    public bool IsActive()
    {
        return m_isActive;
    }



}
