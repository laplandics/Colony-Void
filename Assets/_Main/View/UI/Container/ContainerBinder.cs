using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Container
{
    public class ContainerBinder : MonoBehaviour, IUIBinder<ContainerVm>
    {
        public VisualTreeAsset uiAsset;

        private VisualElement _root; 
        private UIDocument _uiDocument;
        
        public void Bind(ContainerVm vm)
        {
            _uiDocument = vm.Document;
            _root = uiAsset.Instantiate().Q<VisualElement>("ROOT");
            _uiDocument.rootVisualElement.Q<VisualElement>("SPACE").Add(_root);
        }

        public void Unbind()
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("SPACE").Remove(_root);
        }
    }
}