using System;
using System.Collections.Generic;
using Cmd;
using ObservableCollections;
using R3;
using Space;
using View.UI.Resource;
using Constant;

namespace Service
{
    public class ResourceService
    {
        private readonly CommandProcessor _cmd;
        private readonly UI _root;
        private readonly Dictionary<Enums.Resources, ResourceVm> _resourcesMap = new();

        public ResourceService(CommandProcessor cmd, UI root, ObservableList<Data.Proxy.UIElement> elements)
        {
            _cmd = cmd;
            _root = root;
            elements.ForEach(AddResource);
            elements.ObserveAdd().Subscribe(addEvent => AddResource(addEvent.Value));
            elements.ObserveRemove().Subscribe(removeEvent => RemoveResource(removeEvent.Value));
        }

        public bool EarnResource(Enums.Resources typeKey, int amount)
        {
            var command = new CmdCommandEarnResource(typeKey, amount);
            var result = _cmd.Process(command);
            return result;
        }

        public bool SpendResource(Enums.Resources typeKey, int amount)
        {
            var command = new CmdCommandSpendResource(typeKey, amount);
            var result = _cmd.Process(command);
            return result;
        }

        public bool CheckResource(Enums.Resources typeKey, int amount)
        {
            var vm = _resourcesMap[typeKey];
            return vm.Amount.CurrentValue >= amount;
        }

        public Observable<int> ObserveResource(Enums.Resources typeKey)
        {
            if (!_resourcesMap.TryGetValue(typeKey, out var vm))
            { throw new Exception($"Trying to observe resource {typeKey}, which doesn't exist"); }
            var amount = vm.Amount;
            return amount;
        }
        
        private void AddResource(Data.Proxy.UIElement element)
        {
            if (element is not Data.Proxy.Resource resource) return;
            var vm = new ResourceVm(resource);
            _resourcesMap.Add(resource.ResourceType, vm);
            _root.AddUi(vm);
        }

        private void RemoveResource(Data.Proxy.UIElement element)
        {
            if (element is not Data.Proxy.Resource resource) return;
            var vm = _resourcesMap[resource.ResourceType];
            _resourcesMap.Remove(resource.ResourceType);
            _root.RemoveUi(vm);
        }
    }
}