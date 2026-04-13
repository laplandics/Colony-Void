using Cmd;
using Constant;
using Data;
using R3;
using Service;
using Utils;

namespace Boot
{
    public class MenuBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            onExit = new Subject<Unit>();
            
            c.Resolve<CommandProcessor>().Register(new CmdHandlerAddContainer(
                c.Resolve<IDataProvider>().Project.UIElements));
            c.Resolve<CommandProcessor>().Register(new CmdHandlerRemoveContainer(
                c.Resolve<IDataProvider>().Project.UIElements));
            
            c.Resolve<ContainerService>().AddContainer(Enums.Containers.MenuRoot);
            c.Resolve<Cam>().Instantiate(Enums.Cameras.MenuCam);
        }
    }
}