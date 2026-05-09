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
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                collision.GetComponent<PlayerHealth>();

            PlayerCombat playerCombat =
                collision.GetComponent<PlayerCombat>();

            // Savunmada deðilse hasar ver
            if (playerHealth != null)
            {
                if (playerCombat == null || !playerCombat.IsDefensing)
                {
                    playerHealth.TakeDamage(m_damage);
                }
                else
                {
                    Debug.Log("Blocked!");
                }
            }

            // HER DURUMDA efekt oynasýn
            Explode();
        }
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