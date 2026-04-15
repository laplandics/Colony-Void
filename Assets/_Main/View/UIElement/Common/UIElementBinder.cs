using UnityEngine;

namespace View.UIElement
{
    public abstract class UIElementBinder<T> : MonoBehaviour, IUIElementBinder where T : UIElementVm
    {
        protected T Vm;
        
        public void Bind(UIElementVm vm)
        {
            Vm = (T)vm;
            OnBind();
        }

        protected abstract void OnBind();
        protected abstract void OnUnbind();
        
        public void Unbind()
        {
            OnUnbind();
            Destroy(gameObject);
        }
    }

    public interface IUIElementBinder
    {
        public void Bind(UIElementVm vm);
        public void Unbind();
    }
}