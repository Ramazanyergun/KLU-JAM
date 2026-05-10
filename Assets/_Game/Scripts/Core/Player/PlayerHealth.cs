using System;
using UnityEngine;

public class PlayerHealth : Health
{
    public event Action<float> OnHealthIncreased;
    public event Action OnPlayerDeath;
    public Healthbar healthbar;

    SoundManager soundManager;

    void Awake()
    {
        healthbar = GameObject.FindFirstObjectByType<Healthbar>();
        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();

    }
    void Start()
    {
        // Değişkenlerin base class (Health) içinde 'protected' olduğundan emin ol
        m_currentHealth = m_maxHealth;

        // Referans kontrolü: Eğer atanmadıysa hata vermesini engelle
        if (healthbar != null)
        {
            healthbar.SetMaxHealth(m_maxHealth);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} üzerindeki PlayerHealth scriptine bir Healthbar atanmadı!");
        }
    }

    public override void TakeDamage(float damage)
    {
        // Önce temel sınıftaki hasar mantığını (can düşürme) çalıştır
        base.TakeDamage(damage);
        if (soundManager != null)
        {
            soundManager.PlaySFX(soundManager.takedamage); // soundManager'da playerHurt tanımlı olmalı
        }
        // Can barını sadece referans varsa güncelle
        if (healthbar != null)
        {
            healthbar.SetHealth(m_currentHealth);
        }

        if (m_currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Ölüm olayını fırlat (Örn: Kamera sallantısı veya oyun sonu ekranı için)
        OnPlayerDeath?.Invoke();
        if (soundManager != null)
        {
            soundManager.PlaySFX(soundManager.warriordeath); // soundManager'da playerDeath tanımlı olmalı
        }
        Debug.Log("Oyuncu öldü, 4 saniye sonra yok edilecek.");

        // Karakteri yok etmeden önce collider'ı kapatmak iyi bir pratiktir
        // Böylece ölü karakterle etkileşim devam etmez
        if (TryGetComponent<Collider2D>(out var col)) col.enabled = false;

        Destroy(gameObject, 4f);



    }
    private void OnDisable()
    {
        Debug.LogWarning("PlayerHealth DISABLED -> " + gameObject.name, this);
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        m_currentHealth += amount;

        m_currentHealth =
            Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        OnHealthIncreased?.Invoke(m_currentHealth);
    }
}