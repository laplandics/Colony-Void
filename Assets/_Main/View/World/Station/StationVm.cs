using UnityEngine;
using Space;
using Constant;
using R3;

namespace View.Station
{
    public class StationVm : IWorldView
    {
        private StationBinder _binder;
        
        public MonoBehaviour Binder => _binder;
        public string ID { get; }
        public Enums.Entities Type { get; }
        public Enums.Stations StationType { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        
        public StationVm(Data.Proxy.Station proxy)
        {
            ID = proxy.ID;
            Type = proxy.Type;
            StationType = proxy.StationType;
            Position = proxy.Position;
        }
        
        public void OnAdd(Transform root)
        {
            const string directory = Paths.STATION_DIRECTORY_PATH;
            var path = $"{directory}{StationType.ToString()}/{Type.ToString()}";
            var prefab = Resources.Load<StationBinder>(path);
            var binder = Object.Instantiate(prefab, root, false);
            binder.Bind(this);
            _binder = binder;
        }

        public void OnRemove()
        {
            Object.Destroy(_binder.gameObject);
        }
    }
}