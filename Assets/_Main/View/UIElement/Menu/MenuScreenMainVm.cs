using Constant;
using R3;
using Service;

namespace View.UIElement.Menu
{
    public class MenuScreenMainVm : UIElementVm
    {
        private readonly Subject<Unit> _onExit;
        public override Enums.UIElements Key => Enums.UIElements.MenuScreenMain;

        public MenuScreenMainVm(Subject<Unit> onExit)
        {
            _onExit = onExit;
        }

        public void InvokeStartGame()
        {
            _onExit.OnNext(Unit.Default);
        }
    }
}