using _MonsterCouch.Player;
using UnityEngine;

namespace _MonsterCouch.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField]
        private float fleeSpeed = 3f;
        [SerializeField]
        private float fleeDistance = 3f;

        private Transform player;
        private Vector2 screenBounds;
        private float objectWidth;
        private float objectHeight;
        private bool isStopped;

        private SpriteRenderer spriteRenderer;
        private PlayerController cachedPlayerController;
        private float fleeDistanceSqr;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            fleeDistanceSqr = fleeDistance * fleeDistance;
        }

        private void OnDisable()
        {
            isStopped = false;
        }

        public void Initialize(Transform playerTransform, Vector2 bounds)
        {
            player = playerTransform;
            screenBounds = bounds;
            isStopped = false;

            if (spriteRenderer == null) 
                return;
            
            objectWidth = spriteRenderer.bounds.extents.x;
            objectHeight = spriteRenderer.bounds.extents.y;
        }

        private void Update()
        {
            if (isStopped || player == null)
                return;

            // squared distance to avoid expensive sqrt calculation
            Vector2 toPlayer = player.position - transform.position;
            float sqrDistanceToPlayer = toPlayer.sqrMagnitude;

            // player is within flee distance, run away
            if (sqrDistanceToPlayer < fleeDistanceSqr)
            {
                Vector2 fleeDirection = -toPlayer.normalized;
                transform.Translate(fleeDirection * (fleeSpeed * Time.deltaTime));

                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
                pos.y = Mathf.Clamp(pos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
                transform.position = pos;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (cachedPlayerController == null)
                cachedPlayerController = other.GetComponent<PlayerController>();

            if (cachedPlayerController != null)
                isStopped = true;
        }
    }
}