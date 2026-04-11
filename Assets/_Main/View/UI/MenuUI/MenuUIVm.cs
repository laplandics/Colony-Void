using R3;
using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Menu
{
    public class MenuUIVm : IUIView
    {
        private MenuUIBinder _binder;
        
        public UIDocument Document { get; private set; }
        public MonoBehaviour Binder => _binder;
        public Subject<Unit> OnExit { get; }

        public MenuUIVm(Subject<Unit> onExit)
        {
            OnExit = onExit;
        }
        
        public void OnAdd(UIDocument document, Transform root, int order)
        {
            Document = document;
            const string path = Constant.Paths.MENU_UI_PATH;
            var prefab = Resources.Load<MenuUIBinder>(path);
            var binder = Object.Instantiate(prefab, root, false);
            binder.Bind(this);
            _binder = binder;
        }

        public void OnRemove()
        {
            _binder.UnBind();
            Object.Destroy(_binder.gameObject);
        }
    }
}