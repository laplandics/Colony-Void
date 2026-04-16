using Module.UIElement;
using UnityEngine;
using UnityEngine.UI;

namespace View.UIElement
{
    public abstract class WindowBinder<T> : UIElementBinder<T> where T : UIElementVm
    {
        [SerializeField] private DragModule dragModule;
        [SerializeField] protected Button closeButton;
        
        private void Start()
        {
            dragModule.Initialize(GetComponent<RectTransform>());
            dragModule.ModuleStatus = true;
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        protected virtual void OnCloseButtonClicked()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            dragModule.ModuleStatus = false;
            Vm.InvokeClose();
        }
    }
}