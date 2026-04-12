using R3;

namespace Data.State
{
    public class Preferences
    {
        public int VSync { get; set; }
        public int FPS { get; set; }
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
            
            VSync = new ReactiveProperty<int>(Origin.VSync);
            VSync.Skip(1).Subscribe(vSync => Origin.VSync = vSync);
            
            FPS = new ReactiveProperty<int>(Origin.FPS);
            FPS.Skip(1).Subscribe(fps => Origin.FPS = fps);
        }
    }
}
