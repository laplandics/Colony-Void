using System.Linq;
using Settings;

namespace Data
{
    public class Initializer
    {
        private readonly ISettingsProvider _settingsProvider;

        public Initializer(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public State.Project Initialize()
        {
            var projectSettings = _settingsProvider.ProjectSettings;
            var preferences = new State.Preferences
            {
                VSync = _settingsProvider.ApplicationSettings.vSync,
                FPS = _settingsProvider.ApplicationSettings.fps,
                
                CamMoveSpeed = _settingsProvider.ApplicationSettings.camMoveSpeed,
                CamRotateSpeed = _settingsProvider.ApplicationSettings.camRotateSpeed,
                CamZoomSpeed = _settingsProvider.ApplicationSettings.camZoomSpeed,
                CamZoomConstrains = _settingsProvider.ApplicationSettings.camZoomConstrains
            };

            var initialEntities = projectSettings.initialEntities;
            var entities = initialEntities.Select(DataFactory.CreateState).ToList();

            var initialUiElements = projectSettings.initialUIElements;
            var uiElements = initialUiElements.Select(DataFactory.CreateState).ToList();
            
            var state = new State.Project
            {
                Preferences = preferences,
                Entities = entities,
                UIElements = uiElements
            };
            
            return state;
        }
    }
}