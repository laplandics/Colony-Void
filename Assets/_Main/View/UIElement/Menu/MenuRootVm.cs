using Constant;
using R3;

namespace View.UIElement.Menu
{
    public class MenuRootVm : UIRootVm
    {
        private readonly Subject<Unit> _onExit;

        public MenuRootVm(Subject<Unit> onExit) : base(Enums.UIElements.MenuRoot)
        {
            _onExit = onExit;
        }

        public void OpenMenuScreen()
        {
            var vm = new MenuScreenMainVm(_onExit);
            OpenScreen(vm);
        }
    }
}