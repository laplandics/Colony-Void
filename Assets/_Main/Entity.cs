using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Entity
{
    public IEntityConfig EConfig { get; private set; }
    private IEntity _entity;
    
    public Entity(IEntityConfig config)
    {
        EConfig = config;
        var type = Type.GetType(EConfig.EntityType);
        if (type == null) throw new Exception($"Entity {EConfig.EntityType} not found");
        if (new GameObject(EConfig.EntityType).AddComponent(type) is not IEntity instance)
            throw new Exception($"Entity {EConfig.EntityType} can not be created");
        _entity = instance;
        _entity.Init(EConfig);
        _entity.OnCreate();
    }
    
    public T GetInstance<T>() where T : MonoBehaviour, IEntity => _entity is not T e ? null : (T)e.Instance;

    public void Delete()
    {
        if (_entity == null) return;
        _entity.OnDelete();
        Object.Destroy(_entity.Instance);
        _entity = null;
        EConfig = null;
    }
}

public interface IEntity
{
    public MonoBehaviour Instance { get; }
    
    public void Init(IEntityConfig config);
    
    public void OnCreate();
    public void OnDelete();
}

public interface IEntityConfig
{
    public string EntityKey { get; set; }
    public string EntityType { get; }
}