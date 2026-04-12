using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    public class Project
    {
        public Preferences Preferences { get; set; }
        public List<Entity> Entities { get; set; }
        public List<Resource> Resources { get; set; }
    }
}

namespace Data.Proxy
{
    public class Project
    {
        public State.Project Origin { get; }
        public Preferences Preferences { get; }
        
        public ObservableList<Entity> Entities { get; }
        public ObservableList<Resource> Resources { get; }
        
        public Project(State.Project origin)
        {
            Origin = origin;
            
            Preferences = new Preferences(Origin.Preferences);
            
            Entities = new ObservableList<Entity>();
            Origin.Entities.ForEach(entity => Entities.Add(EntityFactory.Create(entity)));
            Entities.ObserveAdd().Subscribe(addEvent => Origin.Entities.Add(addEvent.Value.Origin));
            Entities.ObserveRemove().Subscribe(removeEvent => Origin.Entities.Remove(removeEvent.Value.Origin));
            
            Resources = new ObservableList<Resource>();
            Origin.Resources.ForEach(resource => Resources.Add(new Resource(resource)));
            Resources.ObserveAdd().Subscribe(addEvent => Origin.Resources.Add(addEvent.Value.Origin));
            Resources.ObserveRemove().Subscribe(removeEvent => Origin.Resources.Remove(removeEvent.Value.Origin));
        }
    }
}
