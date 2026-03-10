using System;
using UnityEngine;

public class Unit : MonoBehaviour, IEntity
{
    public string unitType;
    
    public MonoBehaviour Instance => this;
    public UnitConfig Config { get; private set; }

    public void Init(IEntityConfig config)
    { if (config is not UnitConfig unitConf) return; Config = unitConf; }

    public void OnCreate()
    {
        unitType = Config.unitType;
        
        
    }

    public void OnDelete()
    {
        
    }
}

public class UnitVisualizer : UnitSystem
{
    public override void Init(Entity entity)
    {
        base.Init(entity);
    }

    public override void DeInit()
    {
        base.DeInit();
    }
}


[Serializable] public abstract class UnitSystem
{ public string systemName; protected Entity Owner;
public virtual void Init(Entity entity) { Owner = entity; systemName = GetType().Name; } public virtual void DeInit() {} }

