using Constant;
using ObservableCollections;
using R3;
using UnityEngine;

namespace View.Entity
{
    public abstract class EntityVm
    {
        public string ID { get; }
        public Enums.Entities EntityType { get; }
        public MonoBehaviour Binder { get; protected set; }
        public ReadOnlyReactiveProperty<bool> IsSelected { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public IReadOnlyObservableDictionary<Enums.Modules, bool> Modules { get; }
        
        protected EntityVm(Data.Proxy.Entity proxy)
        {
            ID = proxy.ID;
            EntityType = proxy.Type;
            Position = proxy.Position;
            IsSelected = proxy.IsSelected;
            Modules = proxy.Modules;
        }
        
        public abstract void OnAdd(Transform root);
        public abstract void OnRemove();

        protected T LoadBinder<T>(string path, Transform root) where T : MonoBehaviour
        {
            var prefab = Resources.Load<T>(path);
            var binder = Object.Instantiate(prefab, root, false);
            Binder = binder;
            return binder;
        }
    }
}