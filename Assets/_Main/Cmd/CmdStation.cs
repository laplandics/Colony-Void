using System;
using System.Linq;
using ObservableCollections;
using UnityEngine;

namespace Cmd.Game
{
    public class CmdCommandAddStation : ICommand
    {
        public Constant.Enums.Stations TypeKey { get; }
        public Vector3 Position { get; }

        public CmdCommandAddStation(Constant.Enums.Stations typeKey, Vector3 position)
        {
            TypeKey = typeKey;
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
        private readonly ObservableList<Data.Proxy.Station> _stations;

        public CmdHandlerAddStation(ObservableList<Data.Proxy.Station> stations)
        {
            _stations = stations;
        }
        
        public bool Handle(CmdCommandAddStation command)
        {
            var id = Guid.NewGuid().ToString();
            var typeKey = command.TypeKey;
            var position = command.Position;
            
            var samePosStation = _stations.FirstOrDefault(station => station.Position.Value == position);
            if (samePosStation != null)
            { Debug.LogError($"Station on position {position} already exists"); return false; }

            var state = new Data.State.Station
            { id = id, typeKey = typeKey, position = position };
            var proxy = new Data.Proxy.Station(state);
            _stations.Add(proxy);
            return true;
        }
    }

    public class CmdHandlerRemoveStation : ICommandHandler<CmdCommandRemoveStation>
    {
        private readonly ObservableList<Data.Proxy.Station> _stations;

        public CmdHandlerRemoveStation(ObservableList<Data.Proxy.Station> stations)
        {
            _stations = stations;
        }
        
        public bool Handle(CmdCommandRemoveStation command)
        {
            var id = command.ID;
            
            var sameIdStation = _stations.FirstOrDefault(station => station.Id == id);
            if (sameIdStation == null)
            { Debug.LogError($"Station {id} does not exist"); return false; }
            
            _stations.Remove(sameIdStation);
            return true;
        }
    }
}