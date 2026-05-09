using System;
using UnityEngine;

public class EnemyHealth : Health
{
    public event Action OnEnemyDeath;
    
    [Header("UI Reference")]
    public Healthbar healthbar;

    void Awake()
    {
        // Eğer Inspector'dan el ile atanmadıysa otomatik bulmaya çalış
        if (healthbar == null)
        {
            healthbar = GetComponentInChildren<Healthbar>();
        }
    }

    void Start()
    {
        // m_maxHealth ve m_currentHealth değişkenlerinin 
        // ana Health sınıfında 'protected' olduğundan emin olun.
        m_currentHealth = m_maxHealth;

        if (healthbar != null)
        {
            healthbar.SetMaxHealth(m_maxHealth);
        }
    }

    public override void TakeDamage(float damage)
    {
        // Base class içindeki can düşürme ve event fırlatma mantığını çalıştırır
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

        // Ölüm olayını fırlat (Örn: Puan kazanma veya efektler için)
        OnEnemyDeath?.Invoke();

        // Fiziksel etkileşimi kesmek için collider'ı kapat
        if (TryGetComponent<Collider2D>(out var col)) 
        {
            col.enabled = false;
        }

        // Düşmanı 4 saniye sonra yok et
        Destroy(gameObject, 4f);
        
        Debug.Log($"{gameObject.name} yok edildi.");
    }
}