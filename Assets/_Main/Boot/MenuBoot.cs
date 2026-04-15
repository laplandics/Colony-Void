using Constant;
using R3;
using Service;
using Space;
using Utils;
using View.UI.Element;

namespace Boot
{
    public class MenuBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            var exitSubject = new Subject<Unit>();
            
            c.Register(_ => new UIRootVm(Enums.UIElements.MenuRoot), true);
            c.Register(_ => new UIEMenuService(
                c.Resolve<UIRootVm>(),
                c.Resolve<UI>(),
                exitSubject), true);
            
            c.Resolve<UIEMenuService>().OpenScreen();
            c.Resolve<Cam>().Instantiate(Enums.Cameras.MenuCam);
            
            onExit = exitSubject;
        }
    }
}