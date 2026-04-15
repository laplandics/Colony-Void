using Constant;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/UI/Resource")]
    public class ResourceSettings : ScriptableObject
    {
        public Enums.Resources resourceType;
        public int amount;
    }
}