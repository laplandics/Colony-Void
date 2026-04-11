using System;
using System.Collections;
using Cmd;
using Data;
using R3;
using Settings;
using UnityEngine;
using Space;
using UnityEngine.SceneManagement;
using Utils;
using View.UI.Boot;

namespace Boot
{
    public class ProjectBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() => _ = new ProjectBoot();

        private ProjectBoot()
        {
            var di = new DI();
            di.Register(_ => new Coroutines(), true);
            di.Register(_ => new Scenes(), true);
            di.Register(_ => new UI(), true);
            di.Register<ISettingsProvider>(_ => new SoSettingsProvider(), true);
            di.Register(_ => new Initializer(di.Resolve<ISettingsProvider>()), true);
            di.Register<IDataProvider>(_ => new JsonDataProvider(di.Resolve<Initializer>()), true);
            di.Register(_ => new Cam("BootCamera"), true);
            di.Register(_ => new CommandProcessor(), true);
            
            di.Resolve<UI>().AddUi(new BootUIVm());
            di.Resolve<Cam>().Instantiate();
            
    #if UNITY_EDITOR
                
            Debug.LogWarning("Remove temporal editor code (Boot scene)");
            var sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case Constant.Names.MENU_SCENE_NAME:
                    di.Resolve<Coroutines>().Start(LoadMenu(di), out _); return;
                    
                case Constant.Names.GAME_SCENE_NAME:
                    di.Resolve<Coroutines>().Start(LoadGame(di), out _); return;
            }

            if (sceneName != Constant.Names.BOOT_SCENE_NAME) return;
                
    #endif
            
            di.Resolve<Coroutines>().Start(LoadMenu(di), out _);
        }

        private IEnumerator BeforeLoad(DI rootDi)
        {
            var loadSettingsRequest = rootDi.Resolve<ISettingsProvider>().LoadSettings();
            yield return new WaitUntil(() => loadSettingsRequest.IsCompleted);
            if (loadSettingsRequest.IsFaulted || !loadSettingsRequest.Result)
            {throw new Exception("Failed to load settings files."); }

            var dataLoaded = false;
            rootDi.Resolve<IDataProvider>().LoadData().Subscribe(_ => dataLoaded = true);
            yield return new WaitUntil(() => dataLoaded);

            var preferences = rootDi.Resolve<IDataProvider>().Project.Preferences;
            QualitySettings.vSyncCount = preferences.VSync.Value;
            Application.targetFrameRate = preferences.FPS.Value;

            yield return new WaitForSeconds(0.2f);
        }
        
        private IEnumerator LoadMenu(DI rootDi)
        {
            yield return BeforeLoad(rootDi);
            yield return Scenes.Load(Constant.Names.MENU_SCENE_NAME);
            var sceneDi = new DI(rootDi);
            rootDi.Dispose();

            var menu = new MenuBoot();
            menu.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadGame(rootDi), out _);});
        }

        private IEnumerator LoadGame(DI rootDi)
        {
            yield return BeforeLoad(rootDi);
            yield return Scenes.Load(Constant.Names.GAME_SCENE_NAME);
            var sceneDi = new DI(rootDi);
            rootDi.Dispose();
            
            var game = new GameBoot();
            game.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadMenu(rootDi), out _);});
        }
    }
}
