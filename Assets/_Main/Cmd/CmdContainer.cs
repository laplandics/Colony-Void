using System;
using System.Linq;
using Constant;
using ObservableCollections;
using UnityEngine;

namespace Cmd
{
    public class CmdCommandAddContainer : ICommand
    {
        public Enums.Containers ContainerType { get; }

        public CmdCommandAddContainer(Enums.Containers containerType)
        {
            ContainerType = containerType;
        }
    }

    public class CmdCommandRemoveContainer : ICommand
    {
        public string ID { get; }

        public CmdCommandRemoveContainer(string id)
        {
            ID = id;
        }
    }

    public class CmdHandlerAddContainer : ICommandHandler<CmdCommandAddContainer>
    {
        private readonly ObservableList<Data.Proxy.UIElement> _uiElements;
        
        public CmdHandlerAddContainer(ObservableList<Data.Proxy.UIElement> uiElements)
        {
            _uiElements = uiElements;
        }
        
        public bool Handle(CmdCommandAddContainer command)
        {
            var id = Guid.NewGuid().ToString();
            var containerType = command.ContainerType;

            if (_uiElements.Any(element =>
                    element is Data.Proxy.Container container
                    && container.ContainerType == containerType))
            { Debug.LogWarning($"Container of type {containerType} already exists"); }
            
            var proxy = new Data.Proxy.Container(new Data.State.Container
            {
                ID = id,
                Type = Enums.UIElements.Container,
                Visible = true,
                ContainerType = containerType,
                ScreenPosition = Vector2.zero
            });
            
            _uiElements.Add(proxy);
            return true;
        }
    }
    
    public class CmdHandlerRemoveContainer : ICommandHandler<CmdCommandRemoveContainer>
    {
        private readonly ObservableList<Data.Proxy.UIElement> _uiElements;

        public CmdHandlerRemoveContainer(ObservableList<Data.Proxy.UIElement> uiElements)
        {
            _uiElements = uiElements;
        }
        
        public bool Handle(CmdCommandRemoveContainer command)
        {
            var id = command.ID;
            
            var container = _uiElements.FirstOrDefault(element => element.ID == id);
            if (container == null)
            {
                Debug.LogError($"Trying to remove container {id}, which doesn't exist");
                return false;
            }
            
            _uiElements.Remove(container);
            return true;
        }
    }
}