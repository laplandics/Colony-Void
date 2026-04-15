using Constant;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace View.UI.Element
{
    public abstract class UIElementVm
    {
        private readonly Subject<Unit> _onClose = new();
        
        public IUIElementBinder Binder { get; private set; }
        public abstract Enums.UIElements Key { get; }
        public Observable<Unit> OnClose => _onClose;
        
        public void InvokeClose()
        {
            _onClose.OnNext(Unit.Default);
        }

        public void OnAdd(RectTransform root)
        {
            var path = Tools.PathHelper.GetUIElementPath(Key);
            var prefab = Resources.Load<GameObject>(path);
            var binderObj = Object.Instantiate(prefab, root, false);
            var binder = binderObj.GetComponent<IUIElementBinder>();
            binder.Bind(this);
            Binder = binder;
        }

        public void OnRemove()
        {
            Binder.Unbind();
        }
    }
}