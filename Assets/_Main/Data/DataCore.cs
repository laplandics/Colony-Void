using System;
using System.IO;
using R3;
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
        
        private static string Path => Application.persistentDataPath + Constant.Paths.PROJECT_STATE_PATH;

        public JsonDataProvider(Initializer initializer)
        {
            _initializer = initializer;
        }

        public Observable<bool> LoadData()
        {
            Project = null;
            var state = File.Exists(Path) 
                ? JsonUtility.FromJson<State.Project>(File.ReadAllText(Path))
                : CreateProjectState();
            
#if UNITY_EDITOR
            
            Debug.LogWarning("Remove temporal editor code (Reset state)");
            state = CreateProjectState();
            
#endif
            
            Project = new Proxy.Project(state);
            return Observable.Return(true);
        }
        
        public Observable<bool> SaveData()
        {
            var state = Project.Origin;
            var json = JsonUtility.ToJson(state, true);
            File.WriteAllText(Path, json);
            return Observable.Return(true);
        }

        private State.Project CreateProjectState()
        {
            var state = _initializer.Initialize();
            return state;
        }
    }

    [Serializable] public class Entity { public string id; }
}