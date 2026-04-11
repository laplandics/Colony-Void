using System.Collections.Generic;
using Cmd;
using ObservableCollections;
using R3;
using Space;
using UnityEngine;
using View.Station;

namespace Game.Service
{
    public class StationCreator
    {
        private readonly CommandProcessor _cmd;
        private readonly World _root;
        private readonly Dictionary<string, StationVm> _stationVmsMap = new();
        
        public StationCreator(CommandProcessor cmd, World root, ObservableList<Data.Proxy.Station> stations)
        {
            _cmd = cmd;
            _root = root;
            stations.ForEach(CreateStation);
            stations.ObserveAdd().Subscribe(addEvent => CreateStation(addEvent.Value));
            stations.ObserveRemove().Subscribe(removeEvent => DestroyStation(removeEvent.Value));
        }

        public bool AddStation(Constant.Enums.Stations keyType, Vector3 position)
        {
            var command = new Cmd.Game.CmdCommandAddStation(keyType, position);
            var result = _cmd.Process(command);
            return result;
        }

        public bool RemoveStation(string id)
        {
            var command = new Cmd.Game.CmdCommandRemoveStation(id);
            var result = _cmd.Process(command);
            return result;
        }
        
        private void CreateStation(Data.Proxy.Station station)
        {
            var id = station.Id;
            var vm = new StationVm(station);
            _stationVmsMap[id] = vm;
            _root.AddWorld(vm);
        }

        private void DestroyStation(Data.Proxy.Station station)
        {
            var id = station.Id;
            var vm = _stationVmsMap[id];
            _stationVmsMap.Remove(id);
            _root.RemoveWorld(vm);
        }
    }
}
