using Cmd;
using Constant;
using ObservableCollections;
using R3;
using UnityEngine;

namespace Service
{
    public class StationService
    {
        private readonly CommandProcessor _cmd;

        public StationService(CommandProcessor cmd, ObservableList<Data.Proxy.Entity> entities)
        {
            _cmd = cmd;
            
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
            
        }

        private void DestroyStation(Data.Proxy.Entity entity)
        {
            
        }
    }
}