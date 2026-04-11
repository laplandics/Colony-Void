using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Menu
{
    public class MenuButtonsBinder : MonoBehaviour
    {
        private MenuUIVm _vm;
        
        private Button _startButton;
        private Button _settingsButton;
        private Button _exitButton;
        
        public void Bind(MenuUIVm vm)
        {
            _vm = vm;
            
            _startButton = _vm.Document.rootVisualElement.Q<Button>("Start");
            _settingsButton = _vm.Document.rootVisualElement.Q<Button>("Settings");
            _exitButton = _vm.Document.rootVisualElement.Q<Button>("Exit");
            
            _startButton.clicked += StartGame;
        }

        private void StartGame()
        {
            _vm.OnExit.OnNext(Unit.Default);
        }

        public void UnBind()
        {
            _startButton.clicked -= StartGame;
        }
    }
}