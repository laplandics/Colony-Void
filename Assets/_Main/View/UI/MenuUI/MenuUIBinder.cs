using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Menu
{
    public class MenuUIBinder : MonoBehaviour
    {
        public VisualTreeAsset uiAsset;
        public MenuButtonsBinder menuButtonsBinder;
        
        private UIDocument _uiDocument;
        
        public void Bind(MenuUIVm vm)
        {
            _uiDocument = vm.Document;
            _uiDocument.visualTreeAsset = uiAsset;
            menuButtonsBinder.Bind(vm);
        }

        public void UnBind()
        {
            _uiDocument.visualTreeAsset = null;
            menuButtonsBinder.UnBind();
        }
    }
}