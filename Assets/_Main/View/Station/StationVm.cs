using UnityEngine;
using Constant;
using R3;

namespace View.World.Station
{
    public class StationVm
    {
        private StationBinder _binder;
        
        public MonoBehaviour Binder => _binder;
        public string ID { get; }
        public Enums.Entities EntityType { get; }
        public Enums.Stations StationType { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        
        public StationVm(Data.Proxy.Station proxy)
        {
            ID = proxy.ID;
            EntityType = proxy.Type;
            StationType = proxy.StationType;
            Position = proxy.Position;
        }
        
        public void OnAdd(Transform root)
        {
            var directory = EntityType.ToString();
            var station = StationType.ToString();
            var path = $"{directory}/{station}/Station";
            var prefab = Resources.Load<StationBinder>(path);
            var binder = Object.Instantiate(prefab, root, false);
            binder.Bind(this);
            _binder = binder;
        }

        public void OnRemove()
        {
            _binder.Unbind();
            Object.Destroy(_binder.gameObject);
        }
    }
}