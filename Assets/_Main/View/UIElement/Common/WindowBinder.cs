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
            dragModule.OwnerRect = GetComponent<RectTransform>();
            dragModule.Activate();
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        protected virtual void OnCloseButtonClicked()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            dragModule.Deactivate();
            Vm.InvokeClose();
        }
    }
}