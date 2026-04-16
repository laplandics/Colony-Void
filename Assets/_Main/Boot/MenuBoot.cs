using Constant;
using R3;
using Space;
using UnityEngine;
using Utils;
using View.UIElement.Menu;

namespace Boot
{
    public class MenuBoot
    {
        public void Boot(DI c, out Subject<Unit> onExit)
        {
            var exitSubject = new Subject<Unit>();
            
            c.Resolve<UI>().AddRoot(new MenuRootVm(exitSubject)).OpenMenuScreen();
            c.Resolve<Cam>().Instantiate(Enums.Cameras.MenuCam);
            
            Resources.UnloadUnusedAssets();
            onExit = exitSubject;
        }
    }
}