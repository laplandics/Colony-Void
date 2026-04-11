using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Game
{
    public class GameUIVm : IUIView
    {
        private GameUIBinder _binder;
        
        public UIDocument Document { get; private set; }
        public MonoBehaviour Binder => _binder;
        
        public void OnAdd(UIDocument document, Transform root, int order)
        {
            Document = document;
            const string path = Constant.Paths.GAME_UI_PATH;
            var prefab = Resources.Load<GameUIBinder>(path);
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