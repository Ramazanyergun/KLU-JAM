using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public event Action OnAttack;
    public event Action<bool> OnDefenseStatusChanged;

    [Header("Attack Settings")]
    [SerializeField] private float m_attackCooldown = 0.5f;
    [SerializeField] private float m_attackRange = 1.2f;
    [SerializeField] private float m_damage;
    [SerializeField] private Transform m_attackPoint;
    [SerializeField] private LayerMask m_enemyLayer;
    [Header("Defense Settings")]

    private bool m_isDefensing;
    public bool IsDefensing => m_isDefensing;
    private float m_nextAttackTime;
    private bool m_isCurrentlyDefending;

    public void HandleCombat()
    {
        HandleAttack();
        HandleDefense();
    }

    private void HandleAttack()
    {
        if (InputManager.Instance.isAttacking && Time.time >= m_nextAttackTime)
        {
            if (m_isCurrentlyDefending) return;

            ExecuteAttack();
            m_nextAttackTime = Time.time + m_attackCooldown;
        }
    }

    private void ExecuteAttack()
    {
        OnAttack?.Invoke();
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(m_attackPoint.position, m_attackRange, m_enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(m_damage);
        }
    }

    private void HandleDefense()
    {
        bool isInputDefending = InputManager.Instance.isDefensing;

        if (isInputDefending != m_isCurrentlyDefending)
        {
            m_isCurrentlyDefending = isInputDefending;
            OnDefenseStatusChanged?.Invoke(m_isCurrentlyDefending);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_attackPoint.position, m_attackRange);
        }
    }
}