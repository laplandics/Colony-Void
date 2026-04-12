using UnityEngine;

namespace View.Station
{
    public class StationBinder : MonoBehaviour
    {
        public void Bind(StationVm vm)
        {
            transform.position = vm.Position.CurrentValue;
        }
    }
}