using Space;
using UnityEngine;

namespace View.World.Station
{
    public class StationBinder : MonoBehaviour, IWorldBinder<StationVm>
    {
        public void Bind(StationVm vm)
        {
            transform.position = vm.Position.CurrentValue;
        }

        public void Unbind()
        {
            
        }
    }
}