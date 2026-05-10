using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 m_direction;
    private float m_projectileSpeed;
    private float m_damage;

    [SerializeField] private GameObject m_explosionPrefab;

    public void Setup(Vector2 direction, float projectileSpeed, float damage)
    {
        m_direction = direction.normalized;
        m_projectileSpeed = projectileSpeed;
        m_damage = damage;

        float angle = Mathf.Atan2(m_direction.y, m_direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(m_direction * m_projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerMovement movement =
            collision.GetComponentInParent<PlayerMovement>();

        PlayerCombat combat =
            collision.GetComponentInParent<PlayerCombat>();

        //movement.SetSwapHealthAndEnergy(true);
        if (combat != null && combat.IsDefensing)
        {
            Explode();
            return;
        }

        movement?.TakeResourceDamage(m_damage);

        Explode();
    }

    private void Explode()
    {
        // Efekt oluþtur
        if (m_explosionPrefab != null)
        {
            Instantiate(
                m_explosionPrefab,
                transform.position,
                Quaternion.identity);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}