using Constant;
using R3;
using Service;

namespace View.UI.Element
{
    public class MenuScreenMainVm : UIElementVm
    {
        private readonly UIEMenuService _service;
        private readonly Subject<Unit> _onExit;
        public override Enums.UIElements Key => Enums.UIElements.MenuScreenMain;

        public MenuScreenMainVm(UIEMenuService service, Subject<Unit> onExit)
        {
            _service = service;
            _onExit = onExit;
        }
    }
}