using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private int m_poolSize = 20;

    private Queue<GameObject> m_enemyPool =
        new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < m_poolSize; i++)
        {
            GameObject enemy =
                Instantiate(m_enemyPrefab);

            enemy.SetActive(false);

            m_enemyPool.Enqueue(enemy);
        }
    }

    public GameObject GetEnemy()
    {
        if (m_enemyPool.Count > 0)
        {
            GameObject enemy = m_enemyPool.Dequeue();

            enemy.SetActive(true);

            return enemy;
        }

        // Pool yetmezse ekstra oluþtur
        GameObject newEnemy =
            Instantiate(m_enemyPrefab);

        return newEnemy;
    }

    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);

        m_enemyPool.Enqueue(enemy);
    }
}