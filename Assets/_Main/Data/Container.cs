using Constant;
using R3;
using UnityEngine;

namespace Data.State
{
    public class Container : UIElement
    {
        public Enums.Containers ContainerType { get; set; }
        public Vector2 ScreenPosition { get; set; }
    }
}

namespace Data.Proxy
{
    public class Container : UIElement
    {
        public Enums.Containers ContainerType { get; }
        
        public ReactiveProperty<Vector2> ScreenPosition { get; }
        
        public Container(State.Container origin) : base(origin)
        {
            ContainerType = origin.ContainerType;
            
            ScreenPosition = new ReactiveProperty<Vector2>(origin.ScreenPosition);
            ScreenPosition.Skip(1).Subscribe(screenPosition => origin.ScreenPosition = screenPosition);
        }
    }
}