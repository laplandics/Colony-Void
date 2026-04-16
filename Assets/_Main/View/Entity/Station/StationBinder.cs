using Module.Entity;
using R3;
using UnityEngine;

namespace View.Entity.Station
{
    public class StationBinder : MonoBehaviour
    {
        [SerializeField] private SelectionModule selectionModule;
        
        public void Bind(StationVm vm)
        {
            transform.position = vm.Position.CurrentValue;
            selectionModule.Target = vm;
            foreach (var (module, status) in vm.Modules)
            {
                if (module == selectionModule.ModuleKey)
                { selectionModule.ModuleStatus = status; }
            }
        }

        public void Unbind()
        {
            
        }
    }
}