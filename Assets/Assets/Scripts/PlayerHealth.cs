using StarterAssets;
using TMPro;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] TextMeshProUGUI m_healthTxtUI;
    [SerializeField] StarterAssetsInputs m_sai;
    [SerializeField] GameObject m_ingameUI;
    [SerializeField] GameObject m_gameOverUI;
    [SerializeField] GameObject m_player;

    protected override void Awake()
    {
        base.Awake();

        m_sai.SetCursorState(true);

        m_healthTxtUI.text = m_curHealth.ToString();
    }



    public override void TakeDame(int damage)
    {
        base.TakeDame(damage);

        if (m_curHealth > 0)
            m_healthTxtUI.text = m_curHealth.ToString();
        else GameOver();
    }

    public void Recovery(int health)
    {
        Debug.Log("Health recovery: " + health);
        
        m_curHealth += health;
        m_curHealth = Mathf.Clamp(m_curHealth, 0, m_maxHealth);

        m_healthTxtUI.text = m_curHealth.ToString();
    }


    void GameOver()
    {
        m_sai.SetCursorState(false);
        m_ingameUI.SetActive(false);
        m_gameOverUI.SetActive(true);
        m_player.SetActive(false);
    }
}
