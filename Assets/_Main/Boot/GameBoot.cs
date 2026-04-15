using Cmd;
using Constant;
using Data;
using Service;
using R3;
using Space;
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
            c.Register(_ => new StationService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<IDataProvider>().Project.Entities), true);
            c.Register(_ => new ResourceService(
                c.Resolve<CommandProcessor>(),
                c.Resolve<IDataProvider>().Project.Resources), true);
            
            c.Resolve<UI>().AddRoot(new GameRootVm(exitSubject)).OpenGameScreen();
            c.Resolve<World>();
            c.Resolve<StationService>();
            c.Resolve<Cam>().Instantiate(Enums.Cameras.GameCam);
            
            onExit = exitSubject;
        }
    }
}