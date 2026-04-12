using UnityEngine;
using Constant;
using R3;

namespace Data.State
{
    public class Entity
    {
        public string ID { get; set; }
        public Enums.Entities Type { get; set; }
        public Vector3 Position { get; set; }
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

        public Entity(State.Entity origin)
        {
            Origin = origin;
            
            Position = new ReactiveProperty<Vector3>(Origin.Position);
            Position.Skip(1).Subscribe(position => Origin.Position = position);
        }
    }
}