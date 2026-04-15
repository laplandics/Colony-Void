using Constant;
using R3;

namespace View.UIElement.Game
{
    public class GameRootVm : UIRootVm
    {
        private readonly Subject<Unit> _onExit;

        public GameRootVm(Subject<Unit> onExit) : base(Enums.UIElements.GameRoot)
        {
            _onExit = onExit;
        }

        public void OpenGameScreen()
        {
            var vm = new GameScreenMainVm(this);
            OpenScreen(vm);
        }

        public void OpenWindowGameMenu()
        {
            var vm = new GameWindowGameMenuVm(_onExit);
            OpenWindow(vm);
        }

        public void CloseWindowGameMenu()
        {
            CloseWindow(Enums.UIElements.GameWindowGameMenu);
        }
    }
}