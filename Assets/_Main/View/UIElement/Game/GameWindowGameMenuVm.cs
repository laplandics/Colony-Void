using Constant;
using R3;

namespace View.UIElement.Game
{
    public class GameWindowGameMenuVm : UIElementVm
    {
        private readonly Subject<Unit> _onExit;
        public override Enums.UIElements Key => Enums.UIElements.GameWindowGameMenu;

        public GameWindowGameMenuVm(Subject<Unit> onExit)
        {
            _onExit = onExit;
        }
        
        public void InvokeMenuExit()
        {
            _onExit.OnNext(Unit.Default);
        }

        public void InvokeSettingsOpen()
        {
            
        }

        public void InvokeDesktopExit()
        {
            
        }
    }
}