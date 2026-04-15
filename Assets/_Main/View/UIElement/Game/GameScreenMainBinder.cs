using UnityEngine;
using UnityEngine.UI;

namespace View.UI.Element
{
    public class GameScreenMainBinder : UIElementBinder<GameScreenMainVm>
    {
        [SerializeField] private Button windowGameMenuButton;
        
        private readonly bool _windowGameMenuOpened;
        
        protected override void OnBind()
        {
            windowGameMenuButton.onClick.AddListener(OnWindowGameMenuButtonClicked);
        }

        private void OnWindowGameMenuButtonClicked()
        {
            if (!_windowGameMenuOpened)
            {
                Vm.InvokeOpenWindowGameMenu();
            }
            else
            {
                Vm.InvokeCloseWindowGameMenu();
            }
        }

        protected override void OnUnbind()
        {
            windowGameMenuButton.onClick.RemoveListener(OnWindowGameMenuButtonClicked);
        }
    }
}