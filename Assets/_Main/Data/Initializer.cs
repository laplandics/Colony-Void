using System.Collections.Generic;
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
            var preferences = new State.Preferences
            {
                vSync = _settingsProvider.ApplicationSettings.vSync,
                fps = _settingsProvider.ApplicationSettings.fps
            };

            var stations = new List<State.Station>();
            
            var state = new State.Project
            {
                preferences = preferences,
                stations = stations
            };
            
            return state;
        }
    }
}