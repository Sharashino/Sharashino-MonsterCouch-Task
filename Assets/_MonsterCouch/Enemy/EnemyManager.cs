using _MonsterCouch.Player;
using System.Collections;
using UnityEngine;

namespace _MonsterCouch.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private EnemyAI enemyPrefab;
        [SerializeField]
        private int enemyCount = 1000;
        [SerializeField]
        private int spawnBatchSize = 100;

        private Camera mainCamera;
        private Vector2 screenBounds;
        private Transform playerTransform;

        public void SpawnEnemies()
        {
            mainCamera = Camera.main;
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

            // Cache player reference once
            var playerController = FindFirstObjectByType<PlayerController>();
            if (playerController != null)
                playerTransform = playerController.transform;

            StartCoroutine(SpawnEnemiesCoroutine());
        }

        private IEnumerator SpawnEnemiesCoroutine()
        {
            int spawned = 0;
            while (spawned < enemyCount)
            {
                int batchSize = Mathf.Min(spawnBatchSize, enemyCount - spawned);

                for (int i = 0; i < batchSize; i++)
                {
                    SpawnEnemy();
                    spawned++;
                }

                // frame skip to avoid spikes
                yield return null;
            }
        }

        private void SpawnEnemy()
        {
            float x = Random.Range(-screenBounds.x * 0.9f, screenBounds.x * 0.9f);
            float y = Random.Range(-screenBounds.y * 0.9f, screenBounds.y * 0.9f);
            Vector3 spawnPosition = new Vector3(x, y, 0f);

            EnemyAI enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = spawnPosition;
            enemy.transform.rotation = Quaternion.identity;
            enemy.gameObject.SetActive(true);

            // init with cached references
            enemy.Initialize(playerTransform, screenBounds);
        }
    }
}