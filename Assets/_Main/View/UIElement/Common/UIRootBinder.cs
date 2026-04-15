using ObservableCollections;
using R3;
using UnityEngine;

namespace View.UIElement
{
    public class UIRootBinder : MonoBehaviour
    {
        [SerializeField] private RectTransform screen;
        [SerializeField] private RectTransform tokens;
        [SerializeField] private RectTransform windows;
        
        private readonly CompositeDisposable _disposables = new();

        public void Bind(UIRootVm vm)
        {
            _disposables.Add(vm.Screen.Skip(1).Subscribe(sc => sc?.OnAdd(screen)));
            
            foreach (var token in vm.Tokens) token.OnAdd(tokens);
            foreach (var window in vm.Windows) window.OnAdd(windows);
            
            _disposables.Add(vm.Tokens.ObserveAdd().Subscribe(addEvent => addEvent.Value.OnAdd(tokens)));
            _disposables.Add(vm.Tokens.ObserveRemove().Subscribe(removeEvent => removeEvent.Value.OnRemove()));
            
            _disposables.Add(vm.Windows.ObserveAdd().Subscribe(addEvent => addEvent.Value.OnAdd(windows)));
            _disposables.Add(vm.Windows.ObserveRemove().Subscribe(removeEvent => removeEvent.Value.OnRemove()));
        }

        public void Unbind() => _disposables.Dispose();
    }
}