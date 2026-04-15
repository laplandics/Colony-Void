using System;
using UnityEngine;
using View.UI.Element;
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
        
        public void AddRoot(UIRootVm rootVm)
        {
            _currentRoot?.OnRemove();
            rootVm.OnAdd(_uiSpace);
            _currentRoot = rootVm;
        }

        public void Dispose()
        {
            _currentRoot?.OnRemove();
        }
    }

    public class World
    {
        
    }
}