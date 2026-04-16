using System.Linq;
using Constant;
using ObservableCollections;
using UnityEngine;

namespace Cmd.Resource
{
    public class CmdCommandEarnResource : Command
    {
        public Enums.Resources ResourceType { get; }
        public int Amount { get; }

        public CmdCommandEarnResource(Enums.Resources resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }

    public class CmdCommandSpendResource : Command
    {
        public Enums.Resources TypeKey { get; }
        public int Amount { get; }

        public CmdCommandSpendResource(Enums.Resources typeKey, int amount)
        {
            TypeKey = typeKey;
            Amount = amount;
        }
    }
    
    public class CmdHandlerEarnResource : ICommandHandler<CmdCommandEarnResource>
    {
        private readonly ObservableList<Data.Proxy.Resource> _resources;

        public CmdHandlerEarnResource(ObservableList<Data.Proxy.Resource> resources)
        {
            _resources = resources;
        }
        
        public bool Handle(CmdCommandEarnResource command)
        {
            var resourceType = command.ResourceType;
            var amount = command.Amount;

            var res = _resources.FirstOrDefault(resource => resource.ResourceType == resourceType);
            if (res == null)
            {
                var state = new Data.State.Resource
                {
                    ResourceType = resourceType,
                    Amount = 0
                };
                res = new Data.Proxy.Resource(state);
                _resources.Add(res);
            }
            
            res.Amount.Value += amount;
            return true;
        }
    }
    
    public class CmdHandlerSpendResource : ICommandHandler<CmdCommandSpendResource>
    {
        private readonly ObservableList<Data.Proxy.Resource> _resources;
        
        public CmdHandlerSpendResource(ObservableList<Data.Proxy.Resource> resources)
        {
            _resources = resources;
        }
        
        public bool Handle(CmdCommandSpendResource command)
        {
            var resourceType = command.TypeKey;
            var amount = command.Amount;
            
            var res = _resources.FirstOrDefault(resource => resource.ResourceType == resourceType);
            if (res == null)
            {
                Debug.LogError($"Trying to spend resource {resourceType}, which doesn't exist");
                return false;
            }

            if (amount > res.Amount.Value)
            {
                Debug.LogError($"Not enough resources of type {resourceType} " +
                               $"(amount: {res.Amount}, spend: {amount})"); 
                return false;
            }
            
            res.Amount.Value -= amount;
            return true;
        }
    }
}