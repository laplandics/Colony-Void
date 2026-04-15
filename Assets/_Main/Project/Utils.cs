using System;
using System.Collections;
using Constant;
using Data.Proxy;
using R3;
using Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Utils
{
    public class Coroutines
    {
        internal class CoroutineHolder : MonoBehaviour { }
        private readonly CoroutineHolder _coroutineHolder;

        public Coroutines()
        {
            _coroutineHolder = new GameObject("[COROUTINES]").AddComponent<CoroutineHolder>();
            Object.DontDestroyOnLoad(_coroutineHolder.gameObject);
        }
        
        public void Start(IEnumerator enumerator, out Coroutine coroutine,
            MonoBehaviour coroutineHolder = null)
        {
            if (coroutineHolder == null) coroutineHolder = _coroutineHolder;
            coroutine = null; if (enumerator == null) return;
            coroutine = coroutineHolder.StartCoroutine(enumerator);
        }

        public void Stop(Coroutine coroutine, MonoBehaviour coroutineHolder = null)
        {
            if (coroutine == null) return;
            if (coroutineHolder == null) coroutineHolder = _coroutineHolder;
            coroutineHolder.StopCoroutine(coroutine);
        }
    }

    public class Scenes
    {
        public static IEnumerator Load(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(Names.Scenes.BOOT);
            yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }

    public class Cam : IDisposable
    {
        private readonly Preferences _prefs;
        private CompositeDisposable _disposables = new();
        
        public GameObject GetObject { get; private set; }
        public Camera GetCamera { get; private set; }
        
        public Cam(Preferences prefs = null)
        {
            _prefs = prefs;
        }
        
        public void Instantiate(Enums.Cameras type)
        {
            var camName = type.ToString();
            const string directory = Paths.CAMERA_DIRECTORY_PATH;
            var path = directory + camName;
            var camPref = Resources.Load<GameObject>(path);
            GetObject = Object.Instantiate(camPref);
            GetObject.name = camName;
            GetCamera = GetObject.GetComponentInChildren<Camera>();
            GetCamera.tag = "MainCamera";
            Resources.UnloadUnusedAssets();
            if (GetObject.TryGetComponent<CameraController>(out var controller))
            { InitializeController(controller); }
        }

        private void InitializeController(CameraController controller)
        {
            if (_prefs == null) throw new Exception("Camera has a CameraController, but preferences are missing");
            _disposables.Add( _prefs.CamMoveSpeed.Subscribe(value => controller.moveSpeed = value));
            _disposables.Add(_prefs.CamRotateSpeed.Subscribe(value => controller.rotateSpeed = value));
            _disposables.Add(_prefs.CamZoomSpeed.Subscribe(value => controller.zoomSpeed = value));
            _disposables.Add(_prefs.CamZoomConstrains.Subscribe(value => controller.zoomConstrains = value));
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Object.Destroy(GetObject);
        }
    }
}