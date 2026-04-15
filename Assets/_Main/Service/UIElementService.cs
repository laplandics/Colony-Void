using Constant;
using R3;
using Space;
using View.UI.Element;

namespace Service
{
    public abstract class UIElementService
    {
        protected readonly UIRootVm RootVm;
        protected readonly UI Space;

        protected UIElementService(UIRootVm rootVm, UI space)
        {
            RootVm = rootVm;
            Space = space;
            
            Space.AddRoot(RootVm);
        }

        public void CloseAllOnScreen()
        {
            RootVm.CloseAll();
        }
    }
    
    public class UIEBootService : UIElementService
    {
        public UIEBootService(UIRootVm rootVm, UI space) : base(rootVm, space) {}

        public void OpenScreen()
        {
            var vm = new BootScreenMainVm();
            RootVm.OpenScreen(vm);
        }
    }
    
    public class UIEMenuService : UIElementService
    {
        private readonly Subject<Unit> _onExit;

        public UIEMenuService(UIRootVm rootVm, UI space, Subject<Unit> onExit) : base(rootVm, space)
        {
            _onExit = onExit;
            
        }

        public void OpenScreen()
        {
            var vm = new MenuScreenMainVm(this, _onExit);
            RootVm.OpenScreen(vm);
        }
    }
    
    public class UIEGameService : UIElementService
    {
        private readonly Subject<Unit> _onExit;

        public UIEGameService(UIRootVm rootVm, UI space, Subject<Unit> onExit) : base(rootVm, space)
        {
            _onExit = onExit;
        }

        public void OpenScreen()
        {
            var vm = new GameScreenMainVm(this, _onExit);
            RootVm.OpenScreen(vm);
        }

        public void OpenWindowGameMenu()
        {
            var vm = new GameWindowGameMenuVm();
            RootVm.OpenWindow(vm);
        }

        public void CloseWindowGameMenu()
        {
            RootVm.CloseWindow(Enums.UIElements.GameWindowGameMenu);
        }
    }
}