using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Settings/Core/Project")]
    public class ProjectSettings : ScriptableObject
    {
        public List<EntitySettings> initialEntities;
        public List<UIElementSettings> initialUIElements;
    }
}