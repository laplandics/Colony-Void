using System;
using Cmd;
using Cmd.Resource;
using ObservableCollections;
using R3;
using Constant;

namespace Service
{
    public class ResourceService : IDisposable
    {
        private readonly CommandProcessor _cmd;
        private CompositeDisposable _disposables = new();
        
        public ResourceService(CommandProcessor cmd, ObservableList<Data.Proxy.Resource> resources)
        {
            _cmd = cmd;
            
            resources.ForEach(AddResource);
            _disposables.Add(resources.ObserveAdd().Subscribe(addEvent => AddResource(addEvent.Value)));
            _disposables.Add(resources.ObserveRemove().Subscribe(removeEvent => RemoveResource(removeEvent.Value)));
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

        private void AddResource(Data.Proxy.Resource resource)
        {
            
        }

        private void RemoveResource(Data.Proxy.Resource resource)
        {
            
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}