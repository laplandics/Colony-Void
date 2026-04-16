using Constant;
using R3;
using UnityEngine;

namespace Data.State
{
    public class Station : Entity
    {
        public Enums.Stations StationType { get; set; }
        public Vector2Int CellIndex { get; set; }
    }
}

namespace Data.Proxy
{
    public class Station : Entity
    {
        public Enums.Stations StationType { get; }
        public ReactiveProperty<Vector2Int> CellIndex { get; }
        
        public Station(State.Station origin) : base(origin)
        {
            StationType = origin.StationType;
            
            CellIndex = new ReactiveProperty<Vector2Int>(origin.CellIndex);
            CellIndex.Skip(1).Subscribe(cellIndex => CellIndex.Value = cellIndex);
        }
    }
}