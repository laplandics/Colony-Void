using Constant;
using R3;

namespace Data.State
{
    public class UIElement
    {
        public string ID { get; set; }
        public Enums.UIElements Type { get; set; }
        public bool Visible { get; set; }
    }
}

namespace Data.Proxy
{
    public abstract class UIElement
    {
        public State.UIElement Origin { get; }
        public string ID => Origin.ID;
        public Enums.UIElements ElementType => Origin.Type;
        
        public ReactiveProperty<bool> Visible { get; }

        public UIElement(State.UIElement origin)
        {
            Origin = origin;
            
            Visible = new ReactiveProperty<bool>(Origin.Visible);
            Visible.Skip(1).Subscribe(visible => Origin.Visible = visible);
        }
    }
}