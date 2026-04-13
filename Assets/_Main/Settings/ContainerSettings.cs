using Constant;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/UI/Container")]
    public class ContainerSettings : UIElementSettings
    {
        public Enums.Containers containerType;
    }
}