using System;
using UnityEngine;

public class PlayerHealth : Health
{
    public event Action OnHealthChanged;
    public event Action OnPlayerDeath;

    public Healthbar healthbar;


    void Start()
    {
        m_currentHealth = m_maxHealth;
        healthbar.SetmaxHealth(m_maxHealth);
    }


    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        healthbar.SetHealth(m_currentHealth);

        if (m_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        gameObject.SetActive(false);
    }
}
