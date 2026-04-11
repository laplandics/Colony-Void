using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "Settings/Core/Application")]
    public class ApplicationSettings : ScriptableObject
    {
        public int vSync;
        public int fps;
    }
}