using R3;
using Space;
using Utils;
using View.UI.Menu;

namespace Boot
{
    public class MenuBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            onExit = new Subject<Unit>();
            
            c.Register(_ => new Cam("MenuCamera"), true);
            
            c.Resolve<UI>().AddUi(new MenuUIVm(onExit));
            c.Resolve<Cam>().Instantiate();
        }
    }
}