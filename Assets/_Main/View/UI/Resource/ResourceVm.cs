using R3;
using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Resource
{
    public class ResourceVm : IUIView
    {
        public UIDocument Document { get; private set; }
        public ReadOnlyReactiveProperty<int> Amount { get; }
        
        public ResourceVm(Data.Proxy.Resource proxy)
        {
            Amount = proxy.Amount;
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