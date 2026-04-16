using System;
using System.Collections.Generic;
using Cmd;
using Cmd.Entity;
using Cmd.Station;
using Constant;
using ObservableCollections;
using R3;
using Space;
using UnityEngine;
using View.Entity.Station;
using View.UIElement.Game;

namespace Service
{
    public class StationService : IDisposable
    {
        private readonly CommandProcessor _cmd;
        private readonly World _worldRoot;
        private readonly GameRootVm _uiRoot;
        private readonly Utils.Grid _grid;
        
        private readonly CompositeDisposable _disposables = new();
        private readonly Dictionary<string, StationVm> _stationVmsMap = new();
        private readonly Dictionary<StationVm, IDisposable> _openWindowSubscriptionsMap = new();
        
        public StationService
        (
            CommandProcessor cmd,
            World worldRoot,
            GameRootVm uiRoot,
            ObservableList<Data.Proxy.Entity> entities
        )
        {
            _cmd = cmd;
            _worldRoot = worldRoot;
            _uiRoot = uiRoot;

            entities.ForEach(CreateStation);
            _disposables.Add(entities.ObserveAdd().Subscribe(addEvent => CreateStation(addEvent.Value)));
            _disposables.Add(entities.ObserveRemove().Subscribe(removeEvent => DestroyStation(removeEvent.Value)));
        }

        public bool AddStation(Enums.Stations stationType, Vector3 position)
        {
            var command = new CmdCommandAddStation(stationType, position);
            var result = _cmd.Process(command);
            return result;
        }

        public bool RemoveStation(string id)
        {
            var command = new CmdCommandRemoveEntity(id);
            var result = _cmd.Process(command);
            return result;
        }
        
        private void CreateStation(Data.Proxy.Entity entity)
        {
            if (entity is not Data.Proxy.Station station) return;
            var vm = new StationVm(station);
            _stationVmsMap.Add(vm.ID, vm);
            
            _openWindowSubscriptionsMap.Add(vm, vm.IsSelected.Subscribe(selected =>
            { if (selected) OpenStationWindow(station); }));
            
            _worldRoot.AddEntity(vm);
        }

        private void DestroyStation(Data.Proxy.Entity entity)
        {
            if (entity is not Data.Proxy.Station station) return;
            var vm = _stationVmsMap[station.ID];
            _stationVmsMap.Remove(station.ID);

            if (_openWindowSubscriptionsMap.TryGetValue(vm, out var disposable))
            { disposable.Dispose(); }
            _openWindowSubscriptionsMap.Remove(vm);
            
            _worldRoot.RemoveEntity(vm);
        }

        private void OpenStationWindow(Data.Proxy.Station station)
        {
            _uiRoot.OpenWindowStationMenu(station);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
            foreach (var disposable in _openWindowSubscriptionsMap.Values) disposable.Dispose();
            _openWindowSubscriptionsMap.Clear();
        }
    }
}