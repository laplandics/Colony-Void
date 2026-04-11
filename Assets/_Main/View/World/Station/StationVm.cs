using UnityEngine;
using Space;

namespace View.Station
{
    public class StationVm : IWorldView
    {
        private readonly Data.Proxy.Station _proxy;
        private StationBinder _binder;
        public MonoBehaviour Binder => _binder;

        public StationVm(Data.Proxy.Station proxy)
        {
            _proxy = proxy;
        }
        
        public void OnAdd(Transform root)
        {
            const string directory = Constant.Paths.STATION_DIRECTORY_PATH;
            var path = $"{directory}{_proxy.TypeKey.ToString()}/Station";
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