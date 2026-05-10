using System;
using System.Collections;
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
    [SerializeField] private float m_defenseBreakDuration = 3f;

    private bool m_canUseDefense = true;
    private float m_nextAttackTime;
    private bool m_isCurrentlyDefending;

    private PlayerMovement m_playerMovement;

    public bool IsDefensing => m_isCurrentlyDefending;


    SoundManager soundManager;
    private void Awake()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        soundManager = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundManager>();
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
        if (!m_canUseDefense)
        {
            m_isCurrentlyDefending = false;
            OnDefenseStatusChanged?.Invoke(false);
            return;
        }

        bool isInputDefending =
            InputManager.Instance.isDefensing;

        bool wasDefending = m_isCurrentlyDefending;

        bool hasResource =
            m_playerMovement.IsSwapActive
            ? !m_playerMovement.IsDead()
            : m_playerMovement.CurrentEnergy > 0;

        if (isInputDefending && hasResource)
        {
            bool canDefend =
                m_playerMovement.ConsumeDefense(
                    m_defenseCost * Time.deltaTime);

            m_isCurrentlyDefending = canDefend;

            if (!wasDefending)
            {
                soundManager.PlaySFX(soundManager.shield);
            }

            // Resource bittiyse defense break
            if (!canDefend)
            {
                StartCoroutine(DefenseBreakCoroutine());
            }
        }
        else
        {
            m_isCurrentlyDefending = false;
        }

        OnDefenseStatusChanged?.Invoke(m_isCurrentlyDefending);
    }

    private IEnumerator DefenseBreakCoroutine()
    {
        m_canUseDefense = false;

        m_isCurrentlyDefending = false;

        OnDefenseStatusChanged?.Invoke(false);

        Debug.Log("Defense Broken");

        yield return new WaitForSeconds(m_defenseBreakDuration);

        m_canUseDefense = true;

        Debug.Log("Defense Restored");
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