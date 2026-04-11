using System;
using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    [Serializable]
    public class Project
    {
        public Preferences preferences;
        public List<Station> stations;
    }
}

namespace Data.Proxy
{
    public class Project
    {
        public State.Project Origin { get; }
        public Preferences Preferences { get; }
        
        public ObservableList<Station> Stations { get; }
        
        public Project(State.Project origin)
        {
            Origin = origin;
            Preferences = new Preferences(Origin.preferences);
            
            Stations = new ObservableList<Station>();
            Origin.stations.ForEach(station => Stations.Add(new Station(station)));
            Stations.ObserveAdd().Subscribe(addEvent => Origin.stations.Add(addEvent.Value.Origin));
            Stations.ObserveRemove().Subscribe(removeEvent => Origin.stations.Remove(removeEvent.Value.Origin));
        }
    }
}
