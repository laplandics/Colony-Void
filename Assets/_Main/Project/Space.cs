using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Space
{
    public class UI : IDisposable
    {
        private readonly UiContainer _uiContainer;
        private readonly List<IUIView> _currentVms = new();
        
        public UI()
        {
            _uiContainer = new GameObject("[UI]").AddComponent<UiContainer>();
            _uiContainer.Init();
            Object.DontDestroyOnLoad(_uiContainer);
        }
        
        public void AddUi(IUIView uiVm)
        {
            var document = _uiContainer.UIDocument;
            var root = _uiContainer.transform;
            var order = _currentVms.Count;
            uiVm.OnAdd(document, root, order);
            _currentVms.Add(uiVm);
        }

        public void RemoveUi(IUIView uiVm)
        {
            uiVm.OnRemove();
            _currentVms.Remove(uiVm);
        }

        public void Dispose()
        {
            foreach (var vm in _currentVms) vm.OnRemove();
            _currentVms.Clear();
        }

        internal class UiContainer : MonoBehaviour
        {
            public UIDocument UIDocument { get; private set; }

            public void Init()
            {
                UIDocument = gameObject.AddComponent<UIDocument>();
                const string path = Constant.Paths.PROJECT_UI_SETTINGS_PATH;
                var settings = Resources.Load<PanelSettings>(path);
                UIDocument.panelSettings = settings;
            }
        }
    }

    public class World : IDisposable
    {
        private readonly WorldContainer _worldContainer;
        private readonly List<IWorldView> _worldVms = new();

        public World()
        {
            var worldContainer = new GameObject("[WORLD]");
            _worldContainer = worldContainer.AddComponent<WorldContainer>();
        }
        
        public void AddWorld(IWorldView worldVm)
        {
            worldVm.OnAdd(_worldContainer.transform);
            _worldVms.Add(worldVm);
        }

        public void RemoveWorld(IWorldView worldVm)
        {
            worldVm.OnRemove();
            _worldVms.Remove(worldVm);
        }

        public void Dispose()
        {
            foreach (var worldVm in _worldVms) worldVm.OnRemove();
            _worldVms.Clear();
        }
        
        internal class WorldContainer : MonoBehaviour {}
    }

    public interface IUIView
    {
        public UIDocument Document { get; }
        public MonoBehaviour Binder { get; }
        
        public void OnAdd(UIDocument document, Transform root, int order);
        public void OnRemove();
    }

    public interface IWorldView
    {
        public MonoBehaviour Binder { get; }
        
        public void OnAdd(Transform root);
        public void OnRemove();
    }
}