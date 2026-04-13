using System;
using System.Linq;
using Constant;
using ObservableCollections;
using UnityEngine;

namespace Cmd
{
    public class CmdCommandAddStation : ICommand
    {
        public Enums.Stations StationType { get; }
        public Vector3 Position { get; }

        public CmdCommandAddStation(Enums.Stations stationType, Vector3 position)
        {
            StationType = stationType;
            Position = position;
        }
    }

    public class CmdCommandRemoveStation : ICommand
    {
        public string ID { get; }

        public CmdCommandRemoveStation(string id)
        {
            ID = id;
        }
    }

    public class CmdHandlerAddStation : ICommandHandler<CmdCommandAddStation>
    {
        private readonly ObservableList<Data.Proxy.Entity> _entities;
        
        public CmdHandlerAddStation(ObservableList<Data.Proxy.Entity> entities)
        {
            _entities = entities;
        }
        
        public bool Handle(CmdCommandAddStation command)
        {
            var id = Guid.NewGuid().ToString();
            var type = Enums.Entities.Station;
            var position = command.Position;
            var stationType = command.StationType;

            var samePositionStation = _entities.FirstOrDefault(entity => entity.Position.Value == position);
            if (samePositionStation != null)
            {
                Debug.LogError($"Trying to add station on the same position as entity {samePositionStation.ID}");
                return false;
            }
            
            var proxy = new Data.Proxy.Station(new Data.State.Station
            {
                ID = id,
                Type = type,
                Position = position,
                StationType = stationType
            });
            
            _entities.Add(proxy);
            return true;
        }
    }

    public class CmdHandlerRemoveStation : ICommandHandler<CmdCommandRemoveStation>
    {
        private readonly ObservableList<Data.Proxy.Entity> _entities;
        
        public CmdHandlerRemoveStation(ObservableList<Data.Proxy.Entity> entities)
        {
            _entities = entities;
        }
        
        public bool Handle(CmdCommandRemoveStation command)
        {
            var id = command.ID;
            
            var station = _entities.FirstOrDefault(entity => entity.ID == id);
            if (station == null)
            {
                Debug.LogError($"Trying to remove station {id}, which doesn't exist");
                return false;
            }
            
            _entities.Remove(station);
            return true;
        }
    }
}