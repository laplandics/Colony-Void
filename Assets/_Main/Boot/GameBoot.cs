using Cmd;
using Cmd.Game;
using Data;
using Game.Service;
using R3;
using Space;
using UnityEngine;
using Utils;
using View.UI.Game;

namespace Boot
{
    public class GameBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            onExit = new Subject<Unit>();
            
            c.Resolve<CommandProcessor>().Register(new CmdHandlerAddStation(
                c.Resolve<IDataProvider>().Project.Stations));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveStation(
                c.Resolve<IDataProvider>().Project.Stations));
            
            c.Register(_ => new World(), true);
            c.Register(_ => new Cam("GameCamera"), true);
            c.Register(_ => new StationCreator(
                c.Resolve<CommandProcessor>(),
                c.Resolve<World>(),
                c.Resolve<IDataProvider>().Project.Stations), true);
            
            c.Resolve<UI>().AddUi(new GameUIVm());
            c.Resolve<Cam>().Instantiate();
            
            //
            Debug.LogWarning("Remove temporal editor code (Game scene)");
            c.Resolve<StationCreator>().AddStation(Constant.Enums.Stations.Resident, Vector3.zero);
            //
        }
    }
}