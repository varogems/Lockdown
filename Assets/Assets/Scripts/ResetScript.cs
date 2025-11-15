using UnityEngine;

public class ResetScript : MonoBehaviour
{
    [SerializeField] DetectRange m_detectRangeScript;
    [SerializeField] LookatTarget m_lookatTargetScript;
    [SerializeField] Enemy m_enemyScript;
    [SerializeField] ShootingMuzzle m_shootingMuzzle;
    [SerializeField] Health m_healthScript;

    public void Reset()
    {
        m_detectRangeScript.Reset();
        m_enemyScript.Reset();
        m_lookatTargetScript.Reset();
        m_shootingMuzzle.Reset();
        m_healthScript.Reset();
    }
}
