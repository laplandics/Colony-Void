using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Entity/Station")]
    public class StationSettings : ScriptableObject
    {
        public Constant.Enums.Stations typeKey;
    }
}