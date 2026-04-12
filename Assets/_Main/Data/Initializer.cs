using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using Settings;
using UnityEngine;

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
                FPS = _settingsProvider.ApplicationSettings.fps
            };

            var initialEntities = projectSettings.initialEntities;
            var entities = initialEntities.Select(CreateStateFromSettings).ToList();

            var resources = new List<State.Resource>();
            
            var state = new State.Project
            {
                Preferences = preferences,
                Entities = entities,
                Resources = resources
            };
            
            return state;
        }

        private State.Entity CreateStateFromSettings(EntitySettings settings)
        {
            switch (settings.entityType)
            {
                case Enums.Entities.Station:
                    if (settings is not StationSettings stationSettings)
                        throw new Exception($"Invalid entity type: {settings.entityType}");
                    var state = new State.Station
                    {
                        ID = Guid.NewGuid().ToString(),
                        Position = Vector3.zero,
                        Type = settings.entityType,
                        StationType = stationSettings.stationType,
                    };
                    return state;
                
                default: throw new Exception($"Unknown entity type: {settings.entityType.ToString()}");
            }
        }
    }
}