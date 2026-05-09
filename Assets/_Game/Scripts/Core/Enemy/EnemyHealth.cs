using System;
using UnityEngine;

public class EnemyHealth : Health
{
    public event Action OnEnemyDeath;
    
    [Header("UI Reference")]
    public Healthbar healthbar;

    void Awake()
    {
         if (healthbar == null)
        {
            healthbar = GetComponentInChildren<Healthbar>();
        }
    }

    void Start()
    {
       
        m_currentHealth = m_maxHealth;

        if (healthbar != null)
        {
            healthbar.SetMaxHealth(m_maxHealth);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (healthbar != null)
        {
            healthbar.SetHealth(m_currentHealth);
        }

        if (m_currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        // Tüm sistemleri kapat
        EnemyMovement movement = GetComponent<EnemyMovement>();
        EnemyCombat combat = GetComponent<EnemyCombat>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();

        if (movement != null)
            movement.enabled = false;

        if (combat != null)
            combat.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }
        if (col != null)
            col.enabled = false;

        // Ölüm eventi
        OnEnemyDeath?.Invoke();

        // Animasyon bitince destroy et
        Destroy(gameObject, 4f);

        Debug.Log($"{gameObject.name} öldü.");
    }
}