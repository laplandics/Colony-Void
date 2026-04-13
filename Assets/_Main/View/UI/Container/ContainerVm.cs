using Constant;
using Service;
using Space;
using UnityEngine;
using UnityEngine.UIElements;

namespace View.UI.Container
{
    public class ContainerVm : IUIView
    {
        private readonly ContainerService _containerService;
        public UIDocument Document { get; private set; }
        public string ID { get; }
        public Enums.UIElements ElementType { get; }
        public Enums.Containers ContainerType { get; }

        private ContainerBinder _binder;
        
        public ContainerVm(Data.Proxy.Container proxy, ContainerService containerService)
        {
            _containerService = containerService;
            ID = proxy.ID;
            ElementType = proxy.ElementType;
            ContainerType = proxy.ContainerType;
        }
        
        public void OnAdd(UIDocument document, Transform root, int order)
        {
            Document = document;
            var directory = ElementType.ToString();
            var container = ContainerType.ToString();
            var path = $"{directory}/{container}/Container";
            var prefab = Resources.Load<ContainerBinder>(path);
            var binder = Object.Instantiate(prefab, root, false);
            binder.Bind(this);
            _binder = binder;
        }

        public void OnRemove()
        {
            _binder.Unbind();
            Object.Destroy(_binder.gameObject);
        }
    }
}