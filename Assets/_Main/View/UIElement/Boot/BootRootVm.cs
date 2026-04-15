using Constant;

namespace View.UIElement.Boot
{
    public class BootRootVm : UIRootVm
    {
        public BootRootVm() : base(Enums.UIElements.BootRoot) {}
        
        public void OpenBootScreen()
        {
            var vm = new BootScreenMainVm();
            OpenScreen(vm);
        }
    }
}