using System;
using UnityEngine;

public abstract class Health : MonoBehaviour, IDamagable
{

    public event Action<float> OnHealthChanged;
    [SerializeField] protected float m_maxHealth;
    [SerializeField] protected float m_currentHealth;
    protected bool isDead;

    public float CurrentHealth => m_currentHealth;
    public float MaxHealth => m_maxHealth;
    public bool IsDead => isDead;



    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;
        m_currentHealth -= amount;
        m_currentHealth = Mathf.Clamp(m_currentHealth, 0, m_maxHealth);
        OnHealthChanged?.Invoke(amount);

    }


}
