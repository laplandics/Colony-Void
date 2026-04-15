using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using ObservableCollections;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace View.UIElement
{
    public abstract class UIRootVm
    {
        private readonly Enums.UIElements _rootType;
        
        private readonly ReactiveProperty<UIElementVm> _screen = new();
        private readonly ObservableList<UIElementVm> _tokens = new();
        private readonly ObservableList<UIElementVm> _windows = new();
        
        private readonly Dictionary<UIElementVm, IDisposable> _closeSubscriptionsMap = new();
        
        public UIRootBinder Binder { get; private set; }
        public ReadOnlyReactiveProperty<UIElementVm> Screen => _screen;
        public IObservableCollection<UIElementVm> Tokens => _tokens;
        public IObservableCollection<UIElementVm> Windows => _windows;

        protected UIRootVm(Enums.UIElements rootType)
        {
            _rootType = rootType;
        }

        public void OnAdd(Transform space)
        {
            var path = Paths.UI_ROOT_PATH + _rootType;
            var prefab = Resources.Load<UIRootBinder>(path);
            var binder = Object.Instantiate(prefab, space, false);
            binder.Bind(this);
            Binder = binder;
        }

        public void OnRemove()
        {
            CloseAll();
            _screen.Value?.OnRemove();
            _screen.Value = null;
            Binder.Unbind();
            Object.Destroy(Binder.gameObject);
        }
        
        protected void OpenScreen(UIElementVm screenVm)
        {
            _screen.Value?.OnRemove();
            _screen.Value = screenVm;
        }

        protected void OpenToken(UIElementVm tokenVm)
        {
            if (_tokens.Contains(tokenVm))
            { Debug.LogWarning($"Trying to open token, that already exists {tokenVm.Key}"); return; }
            _closeSubscriptionsMap.Add(tokenVm, tokenVm.OnClose.Subscribe(_ =>
            { DisposeUIElement(tokenVm); _tokens.Remove(tokenVm); }));
            _tokens.Add(tokenVm);
        }

        protected void OpenWindow(UIElementVm windowVm)
        {
            if (_windows.Contains(windowVm))
            { Debug.LogWarning($"Trying to open window, that already exists {windowVm.Key}"); return; }
            _closeSubscriptionsMap.Add(windowVm, windowVm.OnClose.Subscribe(_ =>
            { DisposeUIElement(windowVm); _windows.Remove(windowVm); }));
            _windows.Add(windowVm);
        }

        protected void CloseToken(Enums.UIElements tokenKey)
        {
            var token = _windows.FirstOrDefault(token => token.Key == tokenKey);
            if (token == null) return;
            _tokens.Remove(token);
            DisposeUIElement(token);
        }

        protected void CloseWindow(Enums.UIElements windowKey)
        {
            var window = _windows.FirstOrDefault(window => window.Key == windowKey);
            if (window == null) return;
            _windows.Remove(window);
            DisposeUIElement(window);
        }

        protected void CloseAllTokens()
        {
            foreach (var token in _tokens)
            { DisposeUIElement(token); }
            _tokens.Clear();
        }

        protected void CloseAllWindows()
        {
            foreach (var window in _windows)
            { DisposeUIElement(window); }
            _windows.Clear();
        }

        protected void CloseAll()
        {
            CloseAllTokens();
            CloseAllWindows();
        }
        
        private void DisposeUIElement(UIElementVm elementVm)
        {
            if (!_closeSubscriptionsMap.Remove(elementVm, out var subscription)) return;
            subscription.Dispose();
        }
    }
}