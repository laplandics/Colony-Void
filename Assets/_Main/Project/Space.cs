using System;
using UnityEngine;
using View.Entity;
using View.UIElement;
using Object = UnityEngine.Object;

namespace Space
{
    public class UI : IDisposable
    {
        private readonly Transform _uiSpace;
        private UIRootVm _currentRoot;
        
        public UI()
        {
            var space = new GameObject("[UI]");
            Object.DontDestroyOnLoad(space);
            _uiSpace = space.transform;
        }
        
        public T AddRoot<T>(T rootVm) where T : UIRootVm
        {
            _currentRoot = rootVm;
            _currentRoot.OnAdd(_uiSpace);
            return rootVm;
        }
        
        public void Dispose()
        {
            _currentRoot?.OnRemove();
            _currentRoot = null;
        }
    }

    public class World
    {
        private readonly Transform _wordSpace;
        
        public World()
        {
            var space = new GameObject("[WORLD]");
            _wordSpace = space.transform;
        }
        
        public void AddEntity<T>(T entityVm) where T : EntityVm
        {
            entityVm.OnAdd(_wordSpace);
        }

        public void RemoveEntity<T>(T entityVm) where T : EntityVm
        {
            entityVm.OnRemove();
        }
    }
}