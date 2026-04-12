using Data.Proxy;
using R3;
using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.Feature
{
    public class ResourceVm : IUIView
    {
        private readonly Resource _proxy;
        public UIDocument Document { get; private set; }
        public MonoBehaviour Binder { get; }

        public ReadOnlyReactiveProperty<int> Amount => _proxy.Amount;
        
        public ResourceVm(Resource proxy)
        {
            _proxy = proxy;
        }
        
        public void OnAdd(UIDocument document, Transform root, int order)
        {
            Document = document;
        }

        public void OnRemove()
        {
            
        }
    }
}