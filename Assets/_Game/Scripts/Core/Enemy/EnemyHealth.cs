using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : Health
{
    public event Action OnEnemyDeath;

    [Header("UI Reference")]
    public Healthbar healthbar;
    SoundManager soundManager;
    private EnemyPool m_pool;

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
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(SoundManager.Instance.takedamage);
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
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayDeathSound(gameObject.tag);
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
        StartCoroutine(ReturnToPool());

        Debug.Log($"{gameObject.name} öldü.");
    }
    public void SetPool(EnemyPool pool)
    {
        m_pool = pool;
    }

    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(4f);

        m_pool.ReturnEnemy(gameObject);


    }

}