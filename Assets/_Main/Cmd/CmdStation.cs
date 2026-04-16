using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using ObservableCollections;
using Settings;
using UnityEngine;
using Grid = Utils.Grid;

namespace Cmd.Station
{
    public class CmdCommandAddStation : Command
    {
        public Enums.Stations StationType { get; }
        public Vector3 Position { get; }

        public CmdCommandAddStation(Enums.Stations stationType, Vector3 position)
        {
            StationType = stationType;
            Position = position;
        }
    }

    public class CmdHandlerAddStation : ICommandHandler<CmdCommandAddStation>
    {
        private readonly ObservableList<Data.Proxy.Entity> _entities;
        private readonly Grid _grid;

        public CmdHandlerAddStation(ObservableList<Data.Proxy.Entity> entities)
        {
            _entities = entities;
            _grid = new Grid(Grid.GridSize.Grid3X3);
        }
        
        public bool Handle(CmdCommandAddStation command)
        {
            const Enums.Entities type = Enums.Entities.Station;
            var id = Guid.NewGuid().ToString();
            var cellIndex = _grid.GetCellIndex(command.Position);
            var position = _grid.GetCellCenter(cellIndex);
            var stationType = command.StationType;
            
            var stationEntities = _entities.Where(entity => entity.Type == Enums.Entities.Station).ToList();
            var stations = stationEntities.Select(entity => (Data.Proxy.Station)entity).ToList();
            var sameCellStation = stations.FirstOrDefault(station => station.CellIndex.Value == cellIndex);
            if (sameCellStation != null)
            {
                Debug.LogError($"Trying to add station on the same cell as station {sameCellStation.ID}");
                return false;
            }
            
            var settingsPath = $"{type.ToString()}/{stationType.ToString()}/Settings";
            var stationSettings = Resources.Load<StationSettings>(settingsPath);
            var modules = new Dictionary<Enums.Modules, bool>();
            foreach (var moduleSettings in stationSettings.stationModules)
            { modules.Add(moduleSettings.moduleKey, moduleSettings.moduleStatus); }
            
            var proxy = new Data.Proxy.Station(new Data.State.Station
            {
                ID = id,
                Type = type,
                Position = position,
                Modules = modules,
                StationType = stationType,
                CellIndex = cellIndex
            });
            
            _entities.Add(proxy);
            return true;
        }
    }
}