using System;
using System.Linq;
using Constant;
using ObservableCollections;
using UnityEngine;

namespace Cmd
{
    public class CmdCommandEarnResource : ICommand
    {
        public Enums.Resources ResourceType { get; }
        public int Amount { get; }

        public CmdCommandEarnResource(Enums.Resources resourceType, int amount)
        {
            ResourceType = resourceType;
            Amount = amount;
        }
    }

    public class CmdCommandSpendResource : ICommand
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
        private readonly ObservableList<Data.Proxy.UIElement> _uiElements;

        public CmdHandlerEarnResource(ObservableList<Data.Proxy.UIElement> uiElements)
        {
            _uiElements = uiElements;
        }
        
        public bool Handle(CmdCommandEarnResource command)
        {
            var resourceType = command.ResourceType;
            var amount = command.Amount;

            if (_uiElements.FirstOrDefault(element =>
                    element is Data.Proxy.Resource resource
                    && resource.ResourceType == resourceType )
                is not Data.Proxy.Resource res)
            {
                var state = new Data.State.Resource
                {
                    ID = Guid.NewGuid().ToString(),
                    Type = Enums.UIElements.Resource,
                    Visible = false,
                    ResourceType = resourceType,
                    Amount = 0
                };
                res = new Data.Proxy.Resource(state);
                _uiElements.Add(res);
            }
            
            res.Amount.Value += amount;
            return true;
        }
    }
    
    public class CmdHandlerSpendResource : ICommandHandler<CmdCommandSpendResource>
    {
        private readonly ObservableList<Data.Proxy.UIElement> _uiElements;
        
        public CmdHandlerSpendResource(ObservableList<Data.Proxy.UIElement> uiElements)
        {
            _uiElements = uiElements;
        }
        
        public bool Handle(CmdCommandSpendResource command)
        {
            var resourceType = command.TypeKey;
            var amount = command.Amount;
            
            if (_uiElements.FirstOrDefault(element =>
                    element is Data.Proxy.Resource resource
                    && resource.ResourceType == resourceType)
                is not Data.Proxy.Resource res)
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