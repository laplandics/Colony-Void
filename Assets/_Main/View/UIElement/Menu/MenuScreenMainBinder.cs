using UnityEngine;
using UnityEngine.UI;

namespace View.UIElement.Menu
{
    public class MenuScreenMainBinder : UIElementBinder<MenuScreenMainVm>
    {
        [SerializeField] private Button gameButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        
        protected override void OnBind()
        {
            gameButton.onClick.AddListener(OnGameButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnGameButtonClicked()
        {
            Vm.InvokeStartGame();
        }

        private void OnSettingsButtonClicked()
        {
            
        }

        private void OnExitButtonClicked()
        {
            
        }

        protected override void OnUnbind()
        {
            
        }
    }
}