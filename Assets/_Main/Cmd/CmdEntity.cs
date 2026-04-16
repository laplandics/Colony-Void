using System.Linq;
using Constant;
using ObservableCollections;
using UnityEngine;

namespace Cmd.Entity
{
    public class CmdCommandRemoveEntity : Command
    {
        public string ID { get; }

        public CmdCommandRemoveEntity(string id)
        {
            ID = id;
        }
    }
    
    public class CmdCommandSelectEntity : Command
    {
        public string EntityId { get; }

        public CmdCommandSelectEntity(string entityId)
        {
            EntityId = entityId;
        }
    }

    public class CmdCommandDeselectEntity : Command
    {
        public string EntityId { get; }

        public CmdCommandDeselectEntity(string entityId)
        {
            EntityId = entityId;
        }
    }
    
    public class CmdHandlerRemoveEntity : ICommandHandler<CmdCommandRemoveEntity>
    {
        private readonly ObservableList<Data.Proxy.Entity> _entities;
        
        public CmdHandlerRemoveEntity(ObservableList<Data.Proxy.Entity> entities)
        {
            _entities = entities;
        }
        
        public bool Handle(CmdCommandRemoveEntity command)
        {
            var id = command.ID;
            
            var entity = _entities.FirstOrDefault(entity => entity.ID == id);
            if (entity == null)
            {
                Debug.LogError($"Trying to remove entity {id}, that doesn't exist");
                return false;
            }
            
            _entities.Remove(entity);
            return true;
        }
    }

    public class CmdHandlerSelectEntity : ICommandHandler<CmdCommandSelectEntity>
    {
        private readonly IObservableCollection<Data.Proxy.Entity> _entities;

        public CmdHandlerSelectEntity(IObservableCollection<Data.Proxy.Entity> entities)
        {
            _entities = entities;
        }
        
        public bool Handle(CmdCommandSelectEntity command)
        {
            var id = command.EntityId;
            Data.Proxy.Entity entityProxy = null;
            
            foreach (var entity in _entities)
            { if (entity.ID == id) entityProxy = entity; }
            
            if (entityProxy == null)
            { Debug.LogError("Trying to select entity, that does not exist"); return false; }
            
            if (entityProxy.IsSelected.Value)
            {
                Debug.LogError("Trying to select entity, that already selected");
                return false;
            }

            if (!entityProxy.Modules.ContainsKey(Enums.Modules.SelectionModule))
            {
                Debug.LogError("Trying to select entity, that couldn't be selected");
                return false;
            }
            
            entityProxy.IsSelected.Value = true;
            return true;
        }
    }
    
    public class CmdHandlerDeselectEntity : ICommandHandler<CmdCommandDeselectEntity>
    {
        private readonly IObservableCollection<Data.Proxy.Entity> _entities;

        public CmdHandlerDeselectEntity(IObservableCollection<Data.Proxy.Entity> entities)
        {
            _entities = entities;
        }
        
        public bool Handle(CmdCommandDeselectEntity command)
        {
            var id = command.EntityId;
            Data.Proxy.Entity entityProxy = null;
            
            foreach (var entity in _entities)
            { if (entity.ID == id) entityProxy = entity; }
            
            if (entityProxy == null)
            { Debug.LogError("Trying to deselect entity, that does not exist"); return false; }
            
            if (!entityProxy.IsSelected.Value)
            {
                Debug.LogError("Trying to deselect entity, that is not selected");
                return false;
            }
            
            if (!entityProxy.Modules.ContainsKey(Enums.Modules.SelectionModule))
            {
                Debug.LogError("Trying to deselect entity, that shouldn't been selected");
                return false;
            }
            
            entityProxy.IsSelected.Value = false;
            return true;
        }
    }
}