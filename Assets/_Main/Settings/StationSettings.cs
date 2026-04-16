using System;
using Constant;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Entity/Station")]
    public class StationSettings : EntitySettings
    {
        public Enums.Stations stationType;
        public StationModuleSettings[] stationModules;
        
        [Serializable]
        public class StationModuleSettings
        {
            public Enums.Modules moduleKey;
            public bool moduleStatus;
        }
    }

}