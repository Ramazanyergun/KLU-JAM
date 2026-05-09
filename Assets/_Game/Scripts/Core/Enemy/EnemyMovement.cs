using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public event Action<bool> OnMoveStateChanged;

    [Header("Movement Settings")]
    [SerializeField] private float m_moveSpeed = 3f;
    [SerializeField] private float m_chaseSpeed = 5f;
    [SerializeField] private float m_stopDistance = 1.5f;

    [Header("Detection Settings")]
    [SerializeField] private float m_detectionRange = 8f;
    [SerializeField] private LayerMask m_playerLayer;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] m_wayPoints;
    [SerializeField] private float m_waitAtWaypoint = 1.5f;

    private Rigidbody2D m_rb;
    private Transform m_playerTransform;
    private int m_currentWaypointIndex;
    private float m_waitTimer;
    private bool m_isChasing;
    private bool m_isMoving;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) m_playerTransform = player.transform;
    }

    void FixedUpdate()
    {
        HandleDetection();

        if (m_isChasing)
            ChasePlayer();
        else
            Patrol();
    }

    private void HandleDetection()
    {
        if (m_playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, m_playerTransform.position);

        // Oyuncu menzile girerse kovalamaya başla
        m_isChasing = distanceToPlayer <= m_detectionRange;
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, m_playerTransform.position);

        if (distanceToPlayer > m_stopDistance)
        {
            Vector2 direction = (m_playerTransform.position - transform.position).normalized;
            Move(direction, m_chaseSpeed);
        }
        else
        {
            Stop();
        }
    }

    private void Patrol()
    {
        if (m_wayPoints == null || m_wayPoints.Length == 0)
        {
            Stop();
            return;
        }

        Transform targetWaypoint = m_wayPoints[m_currentWaypointIndex];
        float distanceToWaypoint = Vector2.Distance(transform.position, targetWaypoint.position);

        if (distanceToWaypoint > 0.2f)
        {
            Vector2 direction = (targetWaypoint.position - transform.position).normalized;
            Move(direction, m_moveSpeed);
        }
        else
        {
            Stop();
            m_waitTimer += Time.fixedDeltaTime;

            if (m_waitTimer >= m_waitAtWaypoint)
            {
                m_currentWaypointIndex = (m_currentWaypointIndex + 1) % m_wayPoints.Length;
                m_waitTimer = 0;
            }
        }
    }

    private void Move(Vector2 direction, float speed)
    {
        m_rb.linearVelocity = new Vector2(direction.x * speed, m_rb.linearVelocity.y);

        // Yönü çevir (Sprite Flip)
        if (direction.x > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < -0.1f) transform.localScale = new Vector3(-1, 1, 1);

        UpdateMoveState(true);
    }

    private void Stop()
    {
        m_rb.linearVelocity = new Vector2(0, m_rb.linearVelocity.y);
        UpdateMoveState(false);
    }

    private void UpdateMoveState(bool moving)
    {
        if (m_isMoving != moving)
        {
            m_isMoving = moving;
            OnMoveStateChanged?.Invoke(m_isMoving);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Görüş menzilini sarı halka ile göster
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);

        // Durma mesafesini kırmızı halka ile göster
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_stopDistance);
    }
}