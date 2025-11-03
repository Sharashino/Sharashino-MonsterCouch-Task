using _MonsterCouch.Input;
using UnityEngine;

namespace _MonsterCouch.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private float moveSpeed = 5f;

        private Camera mainCamera;
        private Vector2 screenBounds;
        private float objectWidth;
        private float objectHeight;
        private Vector2 moveInput;

        private void Start()
        {
            mainCamera = Camera.main;
            screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                objectWidth = spriteRenderer.bounds.extents.x;
                objectHeight = spriteRenderer.bounds.extents.y;
            }

            if (InputReader.Instance != null)
                InputReader.Instance.MoveEvent += OnMove;
        }

        private void OnDestroy()
        {
            if (InputReader.Instance != null)
                InputReader.Instance.MoveEvent -= OnMove;
        }

        private void OnMove(Vector2 input)
        {
            moveInput = input;
        }

        private void Update()
        {
            // Normalize movement for consistent diagonal speed
            Vector2 movement = moveInput.normalized;
            transform.Translate(movement * moveSpeed * Time.deltaTime);

            // Clamp position to screen bounds
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -screenBounds.x + objectWidth, screenBounds.x - objectWidth);
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);
            transform.position = pos;
        }
    }
}
