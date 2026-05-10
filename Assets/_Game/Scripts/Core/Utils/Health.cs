using System;
using UnityEngine;

public abstract class Health : MonoBehaviour, IDamagable
{
    public event Action<float> OnHealthDecreased;

    [SerializeField] protected float m_maxHealth;
    [SerializeField] protected float m_currentHealth;

    protected bool isDead;

    public float CurrentHealth => m_currentHealth;
    public float MaxHealth => m_maxHealth;
    public bool IsDead => isDead;

    protected void RaiseHealthDecreased()
    {
        OnHealthDecreased?.Invoke(m_currentHealth);
    }

    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        m_currentHealth -= amount;

        m_currentHealth =
            Mathf.Clamp(m_currentHealth, 0, m_maxHealth);

        RaiseHealthDecreased();
    }
}