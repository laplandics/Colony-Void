using System;
using System.Collections.Generic;
using System.IO;
using Constant;
using Newtonsoft.Json;
using R3;
using Settings;
using UnityEngine;

namespace Data
{
    public interface IDataProvider
    {
        public Proxy.Project Project { get; }

        public Observable<bool> LoadData();
        public Observable<bool> SaveData();
    }
    
    public class JsonDataProvider : IDataProvider
    {
        public Proxy.Project Project { get; private set; }
        
        private readonly Initializer _initializer;
        
        private static string Path => Application.persistentDataPath + Paths.PROJECT_STATE_PATH;

        public JsonDataProvider(Initializer initializer)
        {
            _initializer = initializer;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters = new List<JsonConverter> { new Tools.Vector3Converter() }
            };
        }

        public Observable<bool> LoadData()
        {
            Project = null;
            var state = File.Exists(Path) 
                ? JsonConvert.DeserializeObject<State.Project>(File.ReadAllText(Path))
                : CreateProjectState();
            
#if UNITY_EDITOR
            
            Debug.LogWarning("Remove temporal editor code (Reset state)");
            state = CreateProjectState();
            
#endif
            
            Project = new Proxy.Project(state);
            return SaveData();
        }
        
        public Observable<bool> SaveData()
        {
            var state = Project.Origin;
            var json = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(Path, json);
            return Observable.Return(true);
        }

        private State.Project CreateProjectState()
        {
            var state = _initializer.Initialize();
            return state;
        }
    }
    
    public static class EntityFactory
    {
        public static Proxy.Entity Create(State.Entity entity)
        {
            switch (entity.Type)
            {
                case Enums.Entities.Station:
                    return new Proxy.Station(entity as State.Station);
                
                default: throw new Exception($"Unknown entity type: {entity.Type.ToString()}");
            }
        }
    }
}