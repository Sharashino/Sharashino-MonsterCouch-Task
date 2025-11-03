using _MonsterCouch.Player;
using _MonsterCouch.Enemy;
using _MonsterCouch.Input;
using UnityEngine;

namespace _MonsterCouch.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController playerPrefab;
        [SerializeField]
        private EnemyManager enemyManager;
        
        private void Start()
        {
            if (InputReader.Instance != null)
                InputReader.Instance.CancelEvent += OnCancel;
            
            SpawnPlayer(playerPrefab);
            SpawnEnemies();
        }

        private void OnDestroy()
        {
            if (InputReader.Instance != null)
                InputReader.Instance.CancelEvent -= OnCancel;
        }

        private void OnCancel()
        {
            SceneLoader.Instance.LoadMainMenu();
        }
        
        private void SpawnPlayer(PlayerController playerController)
        {
            Instantiate(playerController,  Vector3.zero, Quaternion.identity);
        }
        
        private void SpawnEnemies()
        {
            enemyManager.SpawnEnemies();
        }
    }
}