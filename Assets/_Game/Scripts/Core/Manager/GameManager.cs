using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] m_enemySpawnPoints;
    [SerializeField] private int m_spawnCount = 5;

    private int m_killedEnemyCount;

    [SerializeField] private EnemyPool m_enemyPool;
    private void Awake()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < m_spawnCount; i++)
        {
            int randomIndex =
                Random.Range(0, m_enemySpawnPoints.Length);

            Transform spawnPoint =
                m_enemySpawnPoints[randomIndex];

            GameObject enemy =
                m_enemyPool.GetEnemy();

            enemy.transform.position =
                spawnPoint.position;

            EnemyHealth health =
                enemy.GetComponent<EnemyHealth>();

            if (health != null)
            {
                health.SetPool(m_enemyPool);

                health.OnEnemyDeath += OnEnemyKilled;
            }
        }
    }

    private void OnEnemyKilled()
    {
        m_killedEnemyCount++;

        Debug.Log(
            "Killed Enemy Count: " + m_killedEnemyCount);

        if (m_killedEnemyCount >= m_spawnCount)
        {
            GoNextLevel();
        }
    }

    private void GoNextLevel()
    {
        Debug.Log("LEVEL COMPLETED");

        SceneController.Instance.NextLevel();
    }
}