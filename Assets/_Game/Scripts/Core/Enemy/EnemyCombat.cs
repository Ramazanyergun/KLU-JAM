using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] private bool m_isRanged;
    [SerializeField] private float m_damage;
    [SerializeField] private float m_attackCooldown;

    [SerializeField] private Transform m_targetPoint;
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
        if (SoundManager.Instance != null)
    {
        if (gameObject.CompareTag("Cadi")) 
            SoundManager.Instance.PlaySFX(SoundManager.Instance.bam); // Cadı bam sesi
        else if (gameObject.CompareTag("Goblin"))
            SoundManager.Instance.PlaySFX(SoundManager.Instance.attack); // Goblin attack sesi
        else if (gameObject.CompareTag("Fare"))
            SoundManager.Instance.PlaySFX(SoundManager.Instance.bocukattack); // Fare attack sesi
    }

        //if (isRanged)
        //    ExecuteRangedAttack();
    }

    public void AnimationTriggerStep()
    {
        if (!m_isRanged)
        {
            ExecuteMeleeAttack();
        }
        else { 
            ExecuteRangedAttack();
        }
    }

    private void ExecuteMeleeAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(m_attackTransform.position, m_attackRange, m_attackLayer);
        foreach (Collider2D player in hitPlayer)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>(); 
            PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();

            if (playerCombat != null && playerCombat.IsDefensing)
            {
                Debug.Log("is defensing cant damage");
                return;  
            }

            playerHealth?.TakeDamage(m_damage);
        }
    }

    private void ExecuteRangedAttack()
    {
        if (m_projectilePrefab == null || m_playerTransform == null) return;
        if (gameObject.CompareTag("Cadi") && SoundManager.Instance != null)
        SoundManager.Instance.PlaySFX(SoundManager.Instance.fuf); // Atış sesi

        GameObject instance = Instantiate(m_projectilePrefab, m_attackTransform.position, Quaternion.identity);

        // Mermiye yön ver (Oyuncuya doğru)
        Vector2 direction = (m_targetPoint.position - m_attackTransform.position).normalized;
        // Merminin kendi scripti varsa oraya veriyi aktarabilirsin
        instance.GetComponent<Projectile>().Setup(direction, m_projectileSpeed, m_damage);

   
    }

    private void OnDrawGizmosSelected()
    {
        if (m_attackTransform == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_attackTransform.position, m_attackRange);
    }
}