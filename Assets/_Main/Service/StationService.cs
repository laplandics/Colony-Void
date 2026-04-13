using System.Collections.Generic;
using Cmd;
using Constant;
using ObservableCollections;
using R3;
using Space;
using UnityEngine;
using View.World.Station;

namespace Service
{
    public class StationService
    {
        private readonly CommandProcessor _cmd;
        private readonly World _root;
        private readonly Dictionary<string, StationVm> _stationsVmsMap = new();

        public StationService(CommandProcessor cmd, World root, ObservableList<Data.Proxy.Entity> entities)
        {
            _cmd = cmd;
            _root = root;
            
            entities.ForEach(CreateStation);
            entities.ObserveAdd().Subscribe(addEvent => CreateStation(addEvent.Value));
            entities.ObserveRemove().Subscribe(removeEvent => DestroyStation(removeEvent.Value));
        }

        public bool AddStation(Enums.Stations stationType, Vector3 position)
        {
            var command = new CmdCommandAddStation(stationType, position);
            var result = _cmd.Process(command);
            return result;
        }

        public bool RemoveStation(string id)
        {
            var command = new CmdCommandRemoveStation(id);
            var result = _cmd.Process(command);
            return result;
        }

        private void CreateStation(Data.Proxy.Entity entity)
        {
            if (entity is not Data.Proxy.Station station) return;
            var vm = new StationVm(station);
            _stationsVmsMap.Add(vm.ID, vm);
            _root.AddWorld(vm);
        }

        private void DestroyStation(Data.Proxy.Entity entity)
        {
            if (entity is not Data.Proxy.Station station) return;
            var vm = _stationsVmsMap[station.ID];
            _stationsVmsMap.Remove(station.ID);
            _root.RemoveWorld(vm);
        }
    }
}