using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            yield return SceneManager.LoadSceneAsync(Constant.Names.BOOT_SCENE_NAME);
            yield return null;
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return null;
        }
    }

    public class Cam
    {
        private readonly string _camName;
        
        public Cam(string name) { _camName = name; }
        
        public void Instantiate()
        {
            var camPref = Resources.Load<GameObject>(Constant.Paths.CAMERA_PREFAB_PATH);
            var cam = Object.Instantiate(camPref);
            cam.name = _camName;
            cam.tag = "MainCamera";
            Get = cam.GetComponentInChildren<Camera>();
            Resources.UnloadUnusedAssets();
        }
        
        public Camera Get { get; private set; }
    }
}