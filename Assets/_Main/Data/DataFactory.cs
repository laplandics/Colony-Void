using System;
using Constant;
using Settings;
using UnityEngine;

namespace Data
{
    public static class DataFactory
    {
        public static State.Entity CreateState(EntitySettings settings)
        {
            switch (settings.entityType)
            {
                case Enums.Entities.Station:
                    if (settings is not StationSettings stationSettings)
                        throw new Exception($"Invalid entity type: {settings.entityType}");
                    var stationState = new State.Station
                    {
                        ID = Guid.NewGuid().ToString(),
                        Position = Vector3.zero,
                        Type = settings.entityType,
                        StationType = stationSettings.stationType,
                    };
                    return stationState;
                
                case Enums.Entities.Order:
                    throw new NotImplementedException();
                
                default: throw new Exception($"Unknown entity type: {settings.entityType.ToString()}");
            }
        }
        
        public static State.UIElement CreateState(UIElementSettings settings)
        {
            switch (settings.uiElementType)
            {
                case Enums.UIElements.Container:
                    if (settings is not ContainerSettings containerSettings)
                        throw new Exception($"Invalid element type: {settings.uiElementType}");
                    var containerState = new State.Container
                    {
                        ID = Guid.NewGuid().ToString(),
                        Type = settings.uiElementType,
                        Visible = true,
                        ContainerType = containerSettings.containerType,
                        ScreenPosition = Vector2.zero
                    };
                    return containerState;

                case Enums.UIElements.Resource:
                    if (settings is not ResourceSettings resourceSettings)
                        throw new Exception($"Invalid element type: {settings.uiElementType}");
                    var resourceState = new State.Resource
                    {
                        ID = Guid.NewGuid().ToString(),
                        Type = settings.uiElementType,
                        Visible = true,
                        ResourceType = resourceSettings.resourceType,
                        Amount = resourceSettings.amount,
                    };
                    return resourceState;

                case Enums.UIElements.Order:

                default: throw new Exception($"Unknown element type: {settings.uiElementType.ToString()}");
            }
        }
        
        public static Proxy.Entity CreateProxy(State.Entity entity)
        {
            switch (entity.Type)
            {
                case Enums.Entities.Station:
                    return new Proxy.Station(entity as State.Station);
                
                case Enums.Entities.Order:
                    throw new NotImplementedException();
                
                default: throw new Exception($"Unknown entity type: {entity.Type.ToString()}");
            }
        }

        public static Proxy.UIElement CreateProxy(State.UIElement element)
        {
            switch (element.Type)
            {
                case Enums.UIElements.Container:
                    return new Proxy.Container(element as State.Container);
                
                case Enums.UIElements.Resource:
                    return new Proxy.Resource(element as State.Resource);
                    
                case Enums.UIElements.Order:
                
                default: throw new Exception($"Unknown uiElement type: {element.Type.ToString()}");
            }
        }
    }
}