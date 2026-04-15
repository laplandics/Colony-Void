using UnityEngine;
using UnityEngine.UI;

namespace View.UIElement.Game
{
    public class GameWindowGameMenuBinder : WindowBinder<GameWindowGameMenuVm>
    {
        [SerializeField] private Button menuExitButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button desktopExitButton;
        
        protected override void OnBind()
        {
            menuExitButton.onClick.AddListener(OnMenuExitButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            desktopExitButton.onClick.AddListener(OnDesktopExitButtonClicked);
        }

        private void OnMenuExitButtonClicked()
        {
            Vm.InvokeMenuExit();
        }

        private void OnSettingsButtonClicked()
        {
            Vm.InvokeSettingsOpen();
        }

        private void OnDesktopExitButtonClicked()
        {
            Vm.InvokeDesktopExit();
        }

        protected override void OnUnbind()
        {
            menuExitButton.onClick.RemoveListener(OnMenuExitButtonClicked);
            settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            desktopExitButton.onClick.RemoveListener(OnDesktopExitButtonClicked);
        }
    }
}