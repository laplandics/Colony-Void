using System;
using System.Collections;
using Constant;
using Data.Proxy;
using R3;
using Tools.Cam;
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
        private readonly Inputs _inputs;
        private readonly Preferences _prefs;
        private CompositeDisposable _disposables = new();
        
        public GameObject GetCamContainer { get; private set; }
        public Camera GetCamera { get; private set; }
        public CameraController GetCamController { get; private set; }
        
        public Cam(Inputs inputs, Preferences prefs)
        {
            _inputs = inputs;
            _prefs = prefs;
        }
        
        public void Instantiate(Enums.Cameras type)
        {
            var camName = type.ToString();
            const string directory = Paths.CAMERA_DIRECTORY_PATH;
            var path = directory + camName;
            var camPref = Resources.Load<GameObject>(path);
            GetCamContainer = Object.Instantiate(camPref);
            GetCamContainer.name = camName;
            GetCamera = GetCamContainer.GetComponentInChildren<Camera>();
            GetCamera.tag = "MainCamera";
            Resources.UnloadUnusedAssets();
            
            if (!GetCamContainer.TryGetComponent<CameraController>(out var controller)) return;
            InitializeController(controller);
            GetCamController = controller;
        }

        private void InitializeController(CameraController controller)
        {
            if (_prefs == null) throw new Exception("Camera has a CameraController, but preferences are missing");
            _disposables.Add( _prefs.CamMoveSpeed.Subscribe(value => controller.moveSpeed = value));
            _disposables.Add(_prefs.CamRotateSpeed.Subscribe(value => controller.rotateSpeed = value));
            _disposables.Add(_prefs.CamZoomSpeed.Subscribe(value => controller.zoomSpeed = value));
            _disposables.Add(_prefs.CamZoomConstrains.Subscribe(value => controller.zoomConstrains = value));
            controller.Init(_inputs, GetCamera);
        }

        public void Dispose()
        {
            _disposables.Dispose();
            Object.Destroy(GetCamContainer);
        }
    }

    public class Grid
    {
        public enum GridSize { Grid1X1, Grid3X3 }
        
        private readonly int _cellSize;
        
        public Grid(GridSize size)
        {
            _cellSize = size switch
            {
                GridSize.Grid1X1 => Values.GRID_CELL_SIZE_SMALL,
                GridSize.Grid3X3 => Values.GRID_CELL_SIZE_BIG,
                _ => 1
            };
        }
        
        public Vector2Int GetCellIndex(Vector3 position)
        {
            var x = Mathf.FloorToInt(position.x / _cellSize);
            var y = Mathf.FloorToInt(position.z / _cellSize);
            
            return new Vector2Int(x, y);
        }

        public Vector3 GetCellOrigin(Vector2Int cellIndex)
        {
            var x = cellIndex.x * _cellSize;
            var z = cellIndex.y * _cellSize;
            
            return new Vector3(x, 0f, z);
        }

        public Vector3 GetCellCenter(Vector2Int cellIndex)
        {
            var half = _cellSize / 2f;

            var x = cellIndex.x * _cellSize + half;
            var z = cellIndex.y * _cellSize + half;

            return new Vector3(x, 0f, z);
        }
    }
}