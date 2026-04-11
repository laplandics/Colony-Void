using System;
using R3;
using UnityEngine;

namespace Data.State
{
    [Serializable]
    public class Station : Entity
    {
        public Constant.Enums.Stations typeKey;
        public Vector3 position;
    }
}

namespace Data.Proxy
{
    public class Station
    {
        public State.Station Origin { get; }
        public string Id { get; }
        public Constant.Enums.Stations TypeKey { get; }
        
        public ReactiveProperty<Vector3> Position { get; }
        
        public Station(State.Station origin)
        {
            Origin = origin;
            Id = Origin.id;
            TypeKey = Origin.typeKey;
            
            Position = new ReactiveProperty<Vector3>(Origin.position);
            Position.Skip(1).Subscribe(position => Origin.position = position);
        }
    }
}