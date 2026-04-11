using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Game
{
    public class GameUIBinder : MonoBehaviour
    {
        public VisualTreeAsset uiAsset;

        private UIDocument _uiDocument;
        
        public void Bind(GameUIVm vm)
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