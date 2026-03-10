using System;

public abstract class Manager
{
    public virtual void Init() { }
    public virtual void Run() { }
    public virtual void Stop() { }
}

public class CameraManager : Manager
{
    private Entity _cam;
    
    public override void Run()
    {
        var config = new CamConfig();
        config.entityKey = Guid.NewGuid().ToString();
        config.entityType = nameof(Cam);
        config.components = new[]
            { nameof(CamZoom), nameof(CamMove), nameof(CamRotate) };
        _cam = new Entity(config);
    }

    public override void Stop() { _cam.Delete(); _cam = null; }
}