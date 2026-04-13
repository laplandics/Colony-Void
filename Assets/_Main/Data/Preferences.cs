using R3;
using UnityEngine;

namespace Data.State
{
    public class Preferences
    {
        public int VSync { get; set; }
        public int FPS { get; set; }
        
        public float CamMoveSpeed { get; set; }
        public float CamRotateSpeed { get; set; }
        public float CamZoomSpeed { get; set; }
        public Vector2Int CamZoomConstrains { get; set; }
    }
}

namespace Data.Proxy
{
    public class Preferences
    {
        public State.Preferences Origin { get; }

        public ReactiveProperty<int> VSync { get; }
        public ReactiveProperty<int> FPS { get; }
        
        public ReactiveProperty<float> CamMoveSpeed { get; }
        public ReactiveProperty<float> CamRotateSpeed { get; }
        public ReactiveProperty<float> CamZoomSpeed { get; }
        public ReactiveProperty<Vector2Int> CamZoomConstrains { get; }
        
        public Preferences(State.Preferences origin)
        {
            Origin = origin;
            
            VSync = new ReactiveProperty<int>(Origin.VSync);
            VSync.Skip(1).Subscribe(vSync => Origin.VSync = vSync);
            
            FPS = new ReactiveProperty<int>(Origin.FPS);
            FPS.Skip(1).Subscribe(fps => Origin.FPS = fps);
            
            CamMoveSpeed = new ReactiveProperty<float>(Origin.CamMoveSpeed);
            CamMoveSpeed.Skip(1).Subscribe(camMoveSpeed => Origin.CamMoveSpeed = camMoveSpeed);
            
            CamRotateSpeed = new ReactiveProperty<float>(Origin.CamRotateSpeed);
            CamRotateSpeed.Skip(1).Subscribe(camRotateSpeed => Origin.CamRotateSpeed = camRotateSpeed);
            
            CamZoomSpeed = new ReactiveProperty<float>(Origin.CamZoomSpeed);
            CamZoomSpeed.Skip(1).Subscribe(camZoomSpeed => Origin.CamZoomSpeed = camZoomSpeed);
            
            CamZoomConstrains = new ReactiveProperty<Vector2Int>(Origin.CamZoomConstrains);
            CamZoomConstrains.Skip(1).Subscribe(camZoomConstrains => Origin.CamZoomConstrains = camZoomConstrains);
        }
    }
}
