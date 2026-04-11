using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Boot
{
    public class BootUIBinder : MonoBehaviour
    {
        public VisualTreeAsset uiAsset;

        private UIDocument _uiDocument;
        
        public void Bind(BootUIVm vm)
        {
            _uiDocument = vm.Document;
            _uiDocument.visualTreeAsset = uiAsset;
        }

        public void UnBind()
        {
            _uiDocument.visualTreeAsset = null;
        }
    }
}