using Cmd;
using Constant;
using Data;
using Service;
using R3;
using Space;
using UnityEngine;
using Utils;
using View.UIElement.Game;

namespace Boot
{
    public class GameBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            var exitSubject = new Subject<Unit>();
            
            c.Register(_ => new World(), true);
            c.Register(_ => new GameRootVm(
                exitSubject,
                c.Resolve<SelectionService>()), true);
            c.Register(_ => new SelectionService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<Cam>().GetCamera,
                c.Resolve<Inputs>()), true);
            c.Register(_ => new StationService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<World>(),
                c.Resolve<GameRootVm>(),
                c.Resolve<IDataProvider>().Project.Entities), true);
            c.Register(_ => new ResourceService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<IDataProvider>().Project.Resources), true);

            c.Resolve<Cam>().Instantiate(Enums.Cameras.GameCam);
            c.Resolve<UI>().AddRoot(c.Resolve<GameRootVm>()).OpenGameScreen();
            c.Resolve<SelectionService>().ActivateSelections();
            
            //Test
            
            var rPos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
            c.Resolve<StationService>().AddStation(Enums.Stations.Resident, rPos);
            
            //
            
            Resources.UnloadUnusedAssets();
            onExit = exitSubject;
        }
    }
}