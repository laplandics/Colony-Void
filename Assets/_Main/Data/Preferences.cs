using System;
using R3;

namespace Data.State
{
    [Serializable]
    public class Preferences
    {
        public int vSync;
        public int fps;
    }
}

namespace Data.Proxy
{
    public class Preferences
    {
        public State.Preferences Origin { get; }

        public ReactiveProperty<int> VSync { get; }
        public ReactiveProperty<int> FPS { get; }
        
        public Preferences(State.Preferences origin)
        {
            Origin = origin;
            
            VSync = new ReactiveProperty<int>(Origin.vSync);
            VSync.Skip(1).Subscribe(vSync => Origin.vSync = vSync);
            
            FPS = new ReactiveProperty<int>(Origin.fps);
            FPS.Skip(1).Subscribe(fps => Origin.fps = fps);
        }
    }
}
