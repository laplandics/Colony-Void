using System.Collections.Generic;
using UnityEngine;
using Constant;
using ObservableCollections;
using R3;

namespace Data.State
{
    public class Entity
    {
        public string ID { get; set; }
        public Enums.Entities Type { get; set; }
        public Vector3 Position { get; set; }
        public bool IsSelected { get; set; }
        public Dictionary<Enums.Modules, bool> Modules { get; set; }
    }
}

namespace Data.Proxy
{
    public abstract class Entity
    {
        public State.Entity Origin { get; }
        public string ID => Origin.ID;
        public Enums.Entities Type => Origin.Type;
        
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<bool> IsSelected { get; }
        public ObservableDictionary<Enums.Modules, bool> Modules { get; }

        public Entity(State.Entity origin)
        {
            Origin = origin;
            
            Position = new ReactiveProperty<Vector3>(Origin.Position);
            Position.Skip(1).Subscribe(position => Origin.Position = position);
            
            IsSelected = new ReactiveProperty<bool>(origin.IsSelected);
            IsSelected.Skip(1).Subscribe(isSelected => Origin.IsSelected = isSelected);

            Modules = new ObservableDictionary<Enums.Modules, bool>();
            foreach (var (module, status) in Origin.Modules) Modules.Add(module, status);
            Modules.ObserveAdd().Subscribe(e => Origin.Modules.Add(e.Value.Key, e.Value.Value));
            Modules.ObserveRemove().Subscribe(e => Origin.Modules.Remove(e.Value.Key));
            Modules.ObserveReplace().Subscribe(e =>Origin.Modules[e.OldValue.Key] = e.NewValue.Value);
        }
    }
}