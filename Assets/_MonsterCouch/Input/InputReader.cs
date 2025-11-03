using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

namespace _MonsterCouch.Input
{
    public class InputReader : MonoBehaviour
    {
        public static InputReader Instance { get; private set; }

        // Player events
        public event UnityAction<Vector2> MoveEvent = delegate { };

        // UI events
        public event UnityAction CancelEvent = delegate { };

        private InputSystem_Actions gameInput;
        private PlayerActions playerActions;
        private UIActions uiActions;

        #region Unity Lifecycle

        private void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Initialize input
            gameInput = new InputSystem_Actions();
            playerActions = new PlayerActions(this);
            uiActions = new UIActions(this);

            gameInput.Player.SetCallbacks(playerActions);
            gameInput.UI.SetCallbacks(uiActions);

            Debug.Log("InputReader initialized!");
        }

        private void OnEnable()
        {
            if (gameInput != null)
            {
                gameInput.Player.Enable();
                gameInput.UI.Enable();
            }
        }

        private void OnDisable()
        {
            if (gameInput != null)
            {
                gameInput.Player.Disable();
                gameInput.UI.Disable();
            }
        }

        private void OnDestroy()
        {
            if (gameInput != null && Instance == this)
            {
                gameInput.Player.SetCallbacks(null);
                gameInput.UI.SetCallbacks(null);
                gameInput.Player.Disable();
                gameInput.UI.Disable();
                gameInput.Dispose();
            }
        }

        #endregion

        #region Callback Classes

        private class PlayerActions : InputSystem_Actions.IPlayerActions
        {
            private readonly InputReader reader;

            public PlayerActions(InputReader reader)
            {
                this.reader = reader;
            }

            public void OnMove(InputAction.CallbackContext context)
            {
                reader.MoveEvent?.Invoke(context.ReadValue<Vector2>());
            }
        }

        private class UIActions : InputSystem_Actions.IUIActions
        {
            private readonly InputReader reader;

            public UIActions(InputReader reader)
            {
                this.reader = reader;
            }

            public void OnCancel(InputAction.CallbackContext context)
            {
                if (context.phase == InputActionPhase.Performed)
                    reader.CancelEvent?.Invoke();
            }
        }

    #endregion
    }
}