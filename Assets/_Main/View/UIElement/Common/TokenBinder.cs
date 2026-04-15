using UnityEngine;

namespace View.UIElement
{
    public abstract class TokenBinder<T> : UIElementBinder<T> where T : UIElementVm
    {
        [SerializeField] protected AdvancedButton tokenButton;

        private void Start()
        {
            tokenButton.onLeftClick.AddListener(OnTokenButtonClickedLeft);
            tokenButton.onRightClick.AddListener(OnTokenButtonClickedRight);
        }
        
        protected abstract void OnTokenButtonClickedLeft();
        
        protected abstract void OnTokenButtonClickedRight();
    }
}