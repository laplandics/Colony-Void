using Constant;
using R3;
using Service;

namespace View.UI.Element
{
    public class GameScreenMainVm : UIElementVm
    {
        private readonly UIEGameService _service;
        private readonly Subject<Unit> _onExit;
        public override Enums.UIElements Key => Enums.UIElements.GameScreenMain;

        public GameScreenMainVm(UIEGameService service, Subject<Unit> onExit)
        {
            _service = service;
            _onExit = onExit;
        }
        
        public void InvokeOpenWindowGameMenu()
        {
            
        }

        public void InvokeCloseWindowGameMenu()
        {
            
        }
    }
}