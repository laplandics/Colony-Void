using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Entity/Station")]
    public class StationSettings : EntitySettings
    {
        public Constant.Enums.Stations stationType;
    }
}