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
    [SerializeField] private float m_defenseCost = 15f;

    private float m_nextAttackTime;
    private bool m_isCurrentlyDefending;

    private PlayerMovement m_playerMovement;

    public bool IsDefensing => m_isCurrentlyDefending;


     SoundManager soundManager;
    private void Awake()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        soundManager=GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
    }

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
            soundManager.PlaySFX(soundManager.attack);
            ExecuteAttack();
            m_nextAttackTime = Time.time + m_attackCooldown;

        }
    }

    private void ExecuteAttack()
    {
        OnAttack?.Invoke();

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            m_attackPoint.position,
            m_attackRange,
            m_enemyLayer);
        if (hitEnemies.Length > 0)
        {
            soundManager.PlaySFX(soundManager.takedamage); // SoundManager'a hitImpact eklediğini varsayıyorum
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHealth>()?.TakeDamage(m_damage);
        }
    }

    private void HandleDefense()
    {
        bool isInputDefending = InputManager.Instance.isDefensing;
        bool wasDefending = m_isCurrentlyDefending;
        // Enerji varsa defend yap�labilir
        if (isInputDefending && m_playerMovement.CurrentEnergy > 0)
        {
            m_isCurrentlyDefending = true;
            if (!wasDefending)
        {
            soundManager.PlaySFX(soundManager.shield); 
        }
            // Enerji azalt
            m_playerMovement.UseEnergy(m_defenseCost * Time.deltaTime);
        }
        else
        {
            m_isCurrentlyDefending = false;
        }

        OnDefenseStatusChanged?.Invoke(m_isCurrentlyDefending);
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