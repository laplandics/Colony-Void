using UnityEngine;
using Constant;
using R3;
using View.UIElement.Game;

namespace View.Entity.Station
{
    public class StationVm : EntityVm
    {
        private StationBinder _binder;
        
        public Enums.Stations StationType { get; }
        public ReadOnlyReactiveProperty<Vector2Int> CellIndex { get; }
        
        public StationVm(Data.Proxy.Station proxy) : base(proxy)
        {
            StationType = proxy.StationType;
            CellIndex = proxy.CellIndex;
        }
        
        public override void OnAdd(Transform root)
        {
            var directory = EntityType.ToString();
            var station = StationType.ToString();
            var path = $"{directory}/{station}/Station";
            _binder = LoadBinder<StationBinder>(path, root);
            _binder.Bind(this);
        }

        public override void OnRemove()
        {
            _binder.Unbind();
            Object.Destroy(_binder.gameObject);
        }
    }
}