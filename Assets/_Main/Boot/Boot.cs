using System;
using System.Collections;
using Cmd;
using Constant;
using Data;
using R3;
using Service;
using Settings;
using UnityEngine;
using Space;
using UnityEngine.SceneManagement;
using Utils;

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
            di.Register(_ => new Cam(di.Resolve<IDataProvider>().Project.Preferences), true);
            di.Register(_ => new CommandProcessor(), true);
            
            di.Register(_ => new ContainerService(
                di.Resolve<CommandProcessor>(),
                di.Resolve<UI>(),
                di.Resolve<IDataProvider>().Project.UIElements), true);
            
    #if UNITY_EDITOR
                
            Debug.LogWarning("Remove temporal editor code (Boot scene)");
            var sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case Names.MENU_SCENE_NAME:
                    di.Resolve<Coroutines>().Start(LoadMenu(di), out _); return;
                    
                case Names.GAME_SCENE_NAME:
                    di.Resolve<Coroutines>().Start(LoadGame(di), out _); return;
            }

            if (sceneName != Names.BOOT_SCENE_NAME) return;
                
    #endif
            
            di.Resolve<Coroutines>().Start(LoadMenu(di), out _);
        }

        private IEnumerator BeforeLoad(DI rootDi)
        {
            rootDi.Dispose();
            yield return null;
            
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

            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerAddContainer(
                rootDi.Resolve<IDataProvider>().Project.UIElements));
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveContainer(
                rootDi.Resolve<IDataProvider>().Project.UIElements));
            
            rootDi.Resolve<ContainerService>().AddContainer(Enums.Containers.BootRoot);
            rootDi.Resolve<Cam>().Instantiate(Enums.Cameras.BootCam);
            yield return new WaitForSeconds(0.2f);
        }
        
        private IEnumerator LoadMenu(DI rootDi)
        {
            yield return BeforeLoad(rootDi);
            yield return Scenes.Load(Names.MENU_SCENE_NAME);
            
            yield return null;
            rootDi.Dispose();
            var sceneDi = new DI(rootDi);
            
            var menu = new MenuBoot();
            menu.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadGame(rootDi), out _);});
        }

        private IEnumerator LoadGame(DI rootDi)
        {
            yield return BeforeLoad(rootDi);
            yield return Scenes.Load(Names.GAME_SCENE_NAME);
            
            yield return null;
            rootDi.Dispose();
            var sceneDi = new DI(rootDi);
            
            var game = new GameBoot();
            game.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadMenu(rootDi), out _);});
        }
    }
}
