using Cmd;
using Constant;
using Data;
using Service;
using R3;
using Space;
using Utils;

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
            
            c.Resolve<CommandProcessor>().Register(new CmdHandlerAddContainer(
                c.Resolve<IDataProvider>().Project.UIElements));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveContainer(
                c.Resolve<IDataProvider>().Project.UIElements));
            
            c.Resolve<CommandProcessor>().Register(new CmdHandlerEarnResource(
                c.Resolve<IDataProvider>().Project.UIElements));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerSpendResource(
                c.Resolve<IDataProvider>().Project.UIElements));
            
            c.Register(_ => new World(), true);
            
            c.Register(_ => new StationService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<World>(),
                c.Resolve<IDataProvider>().Project.Entities), true);
            c.Register(_ => new ResourceService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<UI>(),
                c.Resolve<IDataProvider>().Project.UIElements), true);
            
            c.Resolve<StationService>();
            c.Resolve<ContainerService>().AddContainer(Enums.Containers.GameRoot);
            c.Resolve<Cam>().Instantiate(Enums.Cameras.GameCam);
        }
    }
}