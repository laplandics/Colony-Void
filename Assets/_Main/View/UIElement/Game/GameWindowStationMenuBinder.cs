namespace View.UIElement.Game
{
    public class GameWindowStationMenuBinder : WindowBinder<GameWindowStationMenuVm>
    {
        protected override void OnBind()
        {
            
        }
        
        protected override void OnUnbind()
        {
            
        }

        protected override void OnCloseButtonClicked()
        {
            Vm.MarkStationUnselected();
            base.OnCloseButtonClicked();
        }
    }
}