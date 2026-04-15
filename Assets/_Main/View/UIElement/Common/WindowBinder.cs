using UnityEngine;
using UnityEngine.UI;

namespace View.UI.Element
{
    public abstract class WindowBinder<T> : UIElementBinder<T> where T : UIElementVm
    {
        [SerializeField] protected Button dragButton;
        [SerializeField] protected Button closeButton;
        
        private void Start()
        {
            closeButton.onClick.AddListener(OnCloseButtonClicked);
            dragButton.onClick.AddListener(OnDragButtonClicked);
        }

        protected virtual void OnDragButtonClicked()
        {
            
        }
        
        protected virtual void OnCloseButtonClicked()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            Vm.InvokeClose();
        }
    }
}