using _MonsterCouch.Extensions;
using _MonsterCouch.Input;
using UnityEngine.UI;
using UnityEngine;

namespace _MonsterCouch.UI
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField]
        private UINavigationController navigationController;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Button backButton;
        
        private MainMenuController mainMenuController;

        private void Start()
        {
            if (InputReader.Instance != null)
                InputReader.Instance.CancelEvent += OnHideSettings;

            backButton.onClick.AddListener(OnHideSettings);
            OnHideSettings();
        }

        private void OnDestroy()
        {
            if (InputReader.Instance != null)
                InputReader.Instance.CancelEvent -= OnHideSettings;

            backButton.onClick.RemoveListener(OnHideSettings);
        }

        public void SetUp(MainMenuController controller)
        {
            mainMenuController = controller;
        }

        public void OnShowSettings()
        {
            canvasGroup.Enable();
            navigationController.enabled = true;
        }

        private void OnHideSettings()
        {
            canvasGroup.Disable();
            navigationController.enabled = false;
            mainMenuController.OnShowMenu();
        }
    }
}