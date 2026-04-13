using System.Collections.Generic;
using ObservableCollections;
using R3;

namespace Data.State
{
    public class Project
    {
        public Preferences Preferences { get; set; }
        public List<Entity> Entities { get; set; }
        public List<UIElement> UIElements { get; set; }
    }
}

namespace Data.Proxy
{
    public class Project
    {
        public State.Project Origin { get; }
        public Preferences Preferences { get; }
        
        public ObservableList<Entity> Entities { get; }
        public ObservableList<UIElement> UIElements { get; }
        
        public Project(State.Project origin)
        {
            Origin = origin;
            
            Preferences = new Preferences(Origin.Preferences);
            
            Entities = new ObservableList<Entity>();
            Origin.Entities.ForEach(entity => Entities.Add(DataFactory.CreateProxy(entity)));
            Entities.ObserveAdd().Subscribe(addEvent => Origin.Entities.Add(addEvent.Value.Origin));
            Entities.ObserveRemove().Subscribe(removeEvent => Origin.Entities.Remove(removeEvent.Value.Origin));
            
            UIElements = new ObservableList<UIElement>();
            Origin.UIElements.ForEach(element => UIElements.Add(DataFactory.CreateProxy(element)));
            UIElements.ObserveAdd().Subscribe(addEvent => Origin.UIElements.Add(addEvent.Value.Origin));
            UIElements.ObserveRemove().Subscribe(removeEvent => Origin.UIElements.Remove(removeEvent.Value.Origin));
        }
    }
}
