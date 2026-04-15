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
                
                default: throw new Exception($"Unknown entity type: {settings.entityType.ToString()}");
            }
        }

        public static State.Resource CreateState(ResourceSettings settings)
        {
            var resType = settings.resourceType;
            var amount = settings.amount;
            var state = new State.Resource
            {
                ResourceType = resType,
                Amount = amount
            };
            return state;
        }
        
        public static Proxy.Entity CreateProxy(State.Entity entity)
        {
            switch (entity.Type)
            {
                case Enums.Entities.Station:
                    return new Proxy.Station(entity as State.Station);
                
                default: throw new Exception($"Unknown entity type: {entity.Type.ToString()}");
            }
        }
    }
}