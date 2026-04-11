using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Boot
{
    public class BootUIVm : Space.IUIView
    {
        private BootUIBinder _binder;

        public UIDocument Document { get; private set; }
        public MonoBehaviour Binder => _binder;
        
        public void OnAdd(UIDocument document, Transform root, int order)
        {
            Document = document;
            const string path = Constant.Paths.BOOT_UI_PATH;
            var prefab = Resources.Load<BootUIBinder>(path);
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