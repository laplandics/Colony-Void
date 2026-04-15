using UnityEngine;
using UnityEngine.UI;

namespace View.UIElement.Game
{
    public class GameScreenMainBinder : UIElementBinder<GameScreenMainVm>
    {
        [SerializeField] private Button windowGameMenuButton;

        private GameWindowGameMenuVm _gameMenuWindow;
        
        protected override void OnBind()
        {
            windowGameMenuButton.onClick.AddListener(OnWindowGameMenuButtonClicked);
        }

        private void OnWindowGameMenuButtonClicked()
        {
            Vm.InvokeOpenWindowGameMenu();
        }

        protected override void OnUnbind()
        {
            windowGameMenuButton.onClick.RemoveListener(OnWindowGameMenuButtonClicked);
        }
    }
}