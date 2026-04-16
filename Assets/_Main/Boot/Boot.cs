using System;
using System.Collections;
using Cmd;
using Cmd.Entity;
using Cmd.Resource;
using Cmd.Station;
using Constant;
using Data;
using R3;
using Settings;
using Space;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;
using View.UIElement.Boot;

namespace Boot
{
    public class ProjectBoot
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Bootstrap() => _ = new ProjectBoot();

        private ProjectBoot()
        {
            var di = new DI();
            di.Register(_ => new UI(), true);
            di.Register(_ => new Scenes(), true);
            di.Register(_ => new Inputs(), true);
            di.Register(_ => new Coroutines(), true);
            di.Register(_ => new CommandProcessor(), true);
            di.Register<ISettingsProvider>(_ => new SoSettingsProvider(), true);
            di.Register(_ => new Initializer(di.Resolve<ISettingsProvider>()), true);
            di.Register(_ => new Cam(di.Resolve<Inputs>(), di.Resolve<IDataProvider>().Project.Preferences), true);
            di.Register<IDataProvider>(_ => new JsonDataProvider(di.Resolve<Initializer>()), true);
            
            // Remove temporal editor code
            #if UNITY_EDITOR
            
            Debug.LogWarning("Remove temporal editor code (Switch to active scene)");
            var sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case Names.Scenes.MENU:
                    di.Resolve<Coroutines>().Start(LoadMenu(di), out _); return;
                    
                case Names.Scenes.GAME:
                    di.Resolve<Coroutines>().Start(LoadGame(di), out _); return;
            }

            if (sceneName != Names.Scenes.BOOT) return;
            
            #endif
            //
            
            di.Resolve<Coroutines>().Start(LoadMenu(di), out _);
        }

        private bool _isProjectInitialized;
        
        private IEnumerator BeforeFirstLoad(DI rootDi)
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

            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerSelectEntity(
                rootDi.Resolve<IDataProvider>().Project.Entities));
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerDeselectEntity(
                rootDi.Resolve<IDataProvider>().Project.Entities));
            
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveEntity(
                rootDi.Resolve<IDataProvider>().Project.Entities));
            
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerAddStation(
                rootDi.Resolve<IDataProvider>().Project.Entities));
            
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerEarnResource(
                rootDi.Resolve<IDataProvider>().Project.Resources));
            rootDi.Resolve<CommandProcessor>().Register(new CmdHandlerSpendResource(
                rootDi.Resolve<IDataProvider>().Project.Resources));
            
            _isProjectInitialized = true;
            yield return null;
        }
        
        private IEnumerator BeforeEveryLoad(DI rootDi, string sceneName)
        {
            rootDi.Resolve<Inputs>().Disable();
            if (!_isProjectInitialized) yield return BeforeFirstLoad(rootDi);
            
            rootDi.Dispose();
            yield return null;

            rootDi.Resolve<UI>().AddRoot(new BootRootVm()).OpenBootScreen();
            rootDi.Resolve<Cam>().Instantiate(Enums.Cameras.BootCam);
            
            yield return new WaitForSeconds(0.2f);
            yield return Scenes.Load(sceneName);
            
            Resources.UnloadUnusedAssets();
            yield return null;
            rootDi.Resolve<Inputs>().Enable();
        }
        
        private IEnumerator LoadMenu(DI rootDi)
        {
            yield return BeforeEveryLoad(rootDi, Names.Scenes.MENU);
            
            rootDi.Dispose();
            var sceneDi = new DI(rootDi);
            
            var menu = new MenuBoot();
            menu.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadGame(rootDi), out _);});
        }

        private IEnumerator LoadGame(DI rootDi)
        {
            yield return BeforeEveryLoad(rootDi, Names.Scenes.GAME);
            
            rootDi.Dispose();
            var sceneDi = new DI(rootDi);
            
            var game = new GameBoot();
            game.Boot(sceneDi, out var onExit);
            onExit.Subscribe(x => {sceneDi.Dispose(); rootDi.Resolve<Coroutines>().Start(LoadMenu(rootDi), out _);});
        }
    }
}
