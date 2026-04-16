using Constant;
using Data.Proxy;
using R3;
using Service;

namespace View.UIElement.Game
{
    public class GameRootVm : UIRootVm
    {
        private readonly Subject<Unit> _onExit;
        private readonly SelectionService _selectionService;

        public GameRootVm(Subject<Unit> onExit, SelectionService selectionService) :
            base(Enums.UIElements.GameRoot)
        {
            _onExit = onExit;
            _selectionService = selectionService;
        }
        
        public void OpenGameScreen() => OpenScreen(new GameScreenMainVm(this));
        public void OpenWindowGameMenu() => OpenWindow(new GameWindowGameMenuVm(_onExit));
        public void OpenWindowStationMenu(Station proxy) =>
            OpenWindow(new GameWindowStationMenuVm(proxy, _selectionService));
    }
}