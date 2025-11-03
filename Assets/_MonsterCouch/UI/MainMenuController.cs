using UnityEngine.SceneManagement;
using _MonsterCouch.Extensions;
using UnityEngine.UI;
using UnityEngine;

namespace _MonsterCouch.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private SettingsController settingsController;
        [SerializeField]
        private UINavigationController navigationController;
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button exitButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            settingsButton.onClick.AddListener(OnSettingsClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            settingsController.SetUp(this);
            OnShowMenu();
        }

        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(OnPlayClicked);
            settingsButton.onClick.RemoveListener(OnSettingsClicked);
            exitButton.onClick.RemoveListener(OnExitClicked);
        }

        private void OnPlayClicked()
        {
            if (SceneLoader.Instance)
                SceneLoader.Instance.LoadGame();
        }

        private void OnSettingsClicked()
        {
            settingsController.OnShowSettings();
            OnHideMenu();
        }

        private void OnExitClicked()
        {
            if (SceneLoader.Instance)
               SceneLoader.Instance.QuitGame();
        }

        public void OnShowMenu()
        {
            canvasGroup.Enable();
            navigationController.enabled = true;
        }
        
        private void OnHideMenu()
        {
            canvasGroup.Disable();
            navigationController.enabled = false;
        }
    }
}