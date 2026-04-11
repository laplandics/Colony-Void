using System.Threading.Tasks;
using UnityEngine;

namespace Settings
{
    public interface ISettingsProvider
    {
        public ApplicationSettings ApplicationSettings { get; }
        public ProjectSettings ProjectSettings { get; }

        public Task<bool> LoadSettings();
    }
    
    public class SoSettingsProvider : ISettingsProvider
    {
        public ApplicationSettings ApplicationSettings { get; private set; }
        public ProjectSettings ProjectSettings { get; private set; }
        
        public async Task<bool> LoadSettings()
        {
            var loadApplicationSettingsRequest = Resources.LoadAsync<ApplicationSettings>
                (Constant.Paths.APPLICATION_SETTINGS_PATH);
            await loadApplicationSettingsRequest;
            if (loadApplicationSettingsRequest.asset == null)
            { Debug.LogError("Failed to load application settings."); return false; }
            ApplicationSettings = loadApplicationSettingsRequest.asset as ApplicationSettings;

            var loadProjectSettingsRequest = Resources.LoadAsync<ProjectSettings>
                (Constant.Paths.PROJECT_SETTINGS_PATH);
            await loadProjectSettingsRequest;
            if (loadProjectSettingsRequest.asset == null)
            { Debug.LogError("Failed to load project settings."); return false; }
            ProjectSettings = loadProjectSettingsRequest.asset as ProjectSettings;
            
            return true;
        }
    }
}