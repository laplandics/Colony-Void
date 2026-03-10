using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


public static class Core
{
    private static EmptyObject _empty;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Register()
    {
        Data.Load();
        Data.Save();
        
        Input.Reset();
        Coroutine.Reset();
        Update.Reset();
        Event.Reset();
        
        if (_empty != null) { Object.Destroy(_empty.gameObject); _empty = null; }
        _empty = new GameObject().AddComponent<EmptyObject>();
        _empty.gameObject.name = "Empty";
        Object.DontDestroyOnLoad(_empty);
    }

    private static class Boot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void BootGame()
        {
            SetSettings();
            Coroutine.Start(StartGame);
        }

        private static void SetSettings()
        {
            var prefs = Data.Get.preferences.video;
            if (prefs.vSync) { QualitySettings.vSyncCount = 1; Application.targetFrameRate = -1; }
            else { QualitySettings.vSyncCount = 0; Application.targetFrameRate = prefs.frameRate; }
        }
    
        private static IEnumerator StartGame()
        {
            yield return Scene.Change(Scene.GAME);
            var entry = new GameObject("EntryPoint").AddComponent<Entry>();
            yield return null;
            Coroutine.Start(entry.LoadScene);
        }
    }
    
    public static class Coroutine
    {
        private static Dictionary<Func<IEnumerator>, UnityEngine.Coroutine> _routines;
        
        public static UnityEngine.Coroutine Start(Func<IEnumerator> function)
        {
            _routines ??= new Dictionary<Func<IEnumerator>, UnityEngine.Coroutine>();
            if (_routines.Count != 0) ClearNull();
            if (function == null) return null;
            var routine = function();
            var coroutine = _empty.StartCoroutine(routine);
            _routines[function] = coroutine;
            return null;
        }

        private static void ClearNull()
        {
            var nullRoutines = new List<Func<IEnumerator>>();
            foreach (var pair in _routines)
            { if (pair.Value == null) nullRoutines.Add(pair.Key); }
            foreach (var nullRoutine in nullRoutines) { _routines.Remove(nullRoutine); }
        }

        public static void Stop(Func<IEnumerator> function)
        {
            if (!_routines.TryGetValue(function, out var coroutine)) return;
            if (coroutine == null) {_routines.Remove(function); return; }
            _empty.StopCoroutine(coroutine);
            _routines.Remove(function);
            if (_routines.Count == 0) _routines = null;
        }
        
        public static void Reset() => _routines = null;
    }

    public static class Update
    {
        public enum TickType { Update, FixedUpdate, LateUpdate }
        private static Dictionary<Action, TickType> _updates;

        public static void Register(Action action, TickType tickType)
        { _updates ??= new Dictionary<Action, TickType>(); _updates.TryAdd(action, tickType); }

        public static void Tick(TickType type)
        { if (_updates == null || _updates.Count == 0) return; foreach (var update in _updates) 
            { if (update.Value == type) update.Key?.Invoke(); } }
        
        public static void Forget(Action action) => _updates.Remove(action);
        
        public static void Reset() => _updates = null;
    }

    public static class Scene
    {
        public const string BOOT = "Boot";
        public const string GAME = "Game";
        
        public static IEnumerator Change(string sceneName) {
            yield return SceneManager.LoadSceneAsync("Boot"); yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName); yield return null; }
    }

    public static class Input
    {
        private static Inputs _inputs;

        public static Inputs Get
        { get { _inputs ??= new Inputs(); return _inputs; } }
        
        public static void Reset() => _inputs = null;
    }

    public static class Data
    {
        private static global::Data _gameData;
        public static global::Data Get => _gameData;
        
        private static string GetPath => $"{Path.Combine(Application.persistentDataPath, nameof(Data))}.json";

        public static void Save()
        {
            var json = JsonUtility.ToJson(_gameData, true);
            File.WriteAllText(GetPath, json);
        }
    
        public static void Load()
        {
            _gameData = new global::Data();
            if (!File.Exists(GetPath)) return;
            var json = File.ReadAllText(GetPath);
            var data = JsonUtility.FromJson<global::Data>(json);
            _gameData = data;
        }
    }

    public static class Event
    {
        private static readonly Dictionary<Type, Delegate> Subscribers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!Subscribers.TryAdd(type, handler)) Subscribers[type] = (Action<T>)Subscribers[type] + handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!Subscribers.TryGetValue(type, out var existing)) return;
            existing = (Action<T>)existing - handler;
            if (existing == null) Subscribers.Remove(type);
            else Subscribers[type] = existing;
        }

        public static void Invoke<T>(T eventData)
        {
            var type = typeof(T);
            if (Subscribers.TryGetValue(type, out var del)) (del as Action<T>)?.Invoke(eventData);
        }

        public static void Reset() { Subscribers.Clear(); }
    }
}

public class EmptyObject : MonoBehaviour
{
    private void Update() { Core.Update.Tick(Core.Update.TickType.Update); }
    private void FixedUpdate() { Core.Update.Tick(Core.Update.TickType.FixedUpdate); }
    private void LateUpdate() { Core.Update.Tick(Core.Update.TickType.LateUpdate); }
}