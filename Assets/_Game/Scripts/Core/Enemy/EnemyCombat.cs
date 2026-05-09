using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] private bool m_isRanged;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_attackCooldown;
    private float m_lastAttackTime;

    [Header("Melee Settings")]
    [SerializeField] private float m_attackRange;
    [SerializeField] private Transform m_attackTransform;
    [SerializeField] private LayerMask m_attackLayer;

    [Header("Ranged Settings")]
    [SerializeField] private GameObject m_projectilePrefab;
    [SerializeField] private float m_projectileSpeed = 10f;

    private Transform m_playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) m_playerTransform = player.transform;
    }

    void Update()
    {
        m_lastAttackTime += Time.deltaTime;

        // 1. Oyuncu hayatta mı ve sahnede mi kontrol et
        if (m_playerTransform == null) return;

        // 2. Oyuncu ile düşman arasındaki mesafeyi hesapla
        float distanceToPlayer = Vector2.Distance(transform.position, m_playerTransform.position);

        // 3. Eğer oyuncu saldırı menzilindeyse saldırıyı gerçekleştir
        if (distanceToPlayer <= m_attackRange)
        {
            if (m_lastAttackTime >= m_attackCooldown)
            {
                ExecuteAttack(m_isRanged);
                m_lastAttackTime = 0;
            }
        }
    }

    private void ExecuteAttack(bool isRanged)
    {
        OnAttack?.Invoke();

        if (isRanged)
            ExecuteRangedAttack();
    }

    public void AnimationTriggerStep()
    {
        if (!m_isRanged)
        {
            ExecuteMeleeAttack();
        }
    }

    private void ExecuteMeleeAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(m_attackTransform.position, m_attackRange, m_attackLayer);
        foreach (Collider2D player in hitPlayer)
        {

            player.GetComponent<PlayerHealth>()?.TakeDamage(m_damage);
        }
    }

    private void ExecuteRangedAttack()
    {
        if (m_projectilePrefab == null || m_playerTransform == null) return;

        GameObject instance = Instantiate(m_projectilePrefab, m_attackTransform.position, Quaternion.identity);

        // Mermiye yön ver (Oyuncuya doğru)
        Vector2 direction = (m_playerTransform.position - m_attackTransform.position).normalized;

        // Merminin kendi scripti varsa oraya veriyi aktarabilirsin
        // Örn: instance.GetComponent<Projectile>().Setup(direction, m_projectileSpeed, m_damage);

        // Alternatif: Direkt Rigidbody2D ile fırlat (Mermide Rigidbody2D olmalı)
        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * m_projectileSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_attackTransform == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_attackTransform.position, m_attackRange);
    }
}