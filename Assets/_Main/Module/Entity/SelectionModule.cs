using Constant;
using UnityEngine;
using View.Entity;

namespace Module.Entity
{
    public class SelectionModule : MonoBehaviour, IModule
    {
        [SerializeField] private Collider selectionCollider;
        
        public Enums.Modules ModuleKey => Enums.Modules.SelectionModule;
        public bool ModuleStatus { get; set; }
        public EntityVm Target { get; set; }
    }
}