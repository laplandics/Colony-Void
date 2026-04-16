using Constant;
using R3;
using Service;

namespace View.UIElement.Game
{
    public class GameScreenMainVm : UIElementVm
    {
        private readonly GameRootVm _root;
        public override Enums.UIElements Key => Enums.UIElements.GameScreenMain;

        public GameScreenMainVm(GameRootVm root)
        {
            _root = root;
        }
        
        public void InvokeOpenWindowGameMenu()
        {
            _root.OpenWindowGameMenu();
        }
    }
}