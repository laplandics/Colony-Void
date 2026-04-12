using System;
using System.Collections.Generic;
using Cmd;
using ObservableCollections;
using R3;
using Space;
using View.Feature;
using Constant;

namespace Game.Service
{
    public class ResourceService
    {
        private readonly CommandProcessor _cmd;
        private readonly UI _root;
        private readonly Dictionary<Enums.Resources, ResourceVm> _resourcesMap = new();

        public ResourceService(CommandProcessor cmd, UI root, ObservableList<Data.Proxy.Resource> resources)
        {
            _cmd = cmd;
            _root = root;
            resources.ForEach(AddResource);
            resources.ObserveAdd().Subscribe(addEvent => AddResource(addEvent.Value));
            resources.ObserveRemove().Subscribe(removeEvent => RemoveResource(removeEvent.Value));
        }

        public bool EarnResource(Enums.Resources typeKey, int amount)
        {
            var command = new Cmd.Game.CmdCommandEarnResource(typeKey, amount);
            var result = _cmd.Process(command);
            return result;
        }

        public bool SpendResource(Enums.Resources typeKey, int amount)
        {
            var command = new Cmd.Game.CmdCommandSpendResource(typeKey, amount);
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
        
        private void AddResource(Data.Proxy.Resource resource)
        {
            var vm = new ResourceVm(resource);
            _resourcesMap.Add(resource.TypeKey, vm);
            _root.AddUi(vm);
        }

        private void RemoveResource(Data.Proxy.Resource resource)
        {
            var vm = _resourcesMap[resource.TypeKey];
            _resourcesMap.Remove(resource.TypeKey);
            _root.RemoveUi(vm);
        }
    }
}