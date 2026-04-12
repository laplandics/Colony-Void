using Cmd;
using Cmd.Game;
using Data;
using Game.Service;
using R3;
using Space;
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
                c.Resolve<IDataProvider>().Project.Entities));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveStation(
                c.Resolve<IDataProvider>().Project.Entities));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerEarnResource(
                c.Resolve<IDataProvider>().Project.Resources));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerSpendResource(
                c.Resolve<IDataProvider>().Project.Resources));
            
            c.Register(_ => new World(), true);
            c.Register(_ => new Cam("GameCamera"), true);
            
            c.Register(_ => new StationService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<World>(),
                c.Resolve<IDataProvider>().Project.Entities), true);
            c.Register(_ => new ResourceService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<UI>(),
                c.Resolve<IDataProvider>().Project.Resources), true);
            
            c.Resolve<StationService>();
            c.Resolve<UI>().AddUi(new GameUIVm());
            c.Resolve<Cam>().Instantiate();
        }
    }
}