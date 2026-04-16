using Constant;
using Service;

namespace View.UIElement.Game
{
    public class GameWindowStationMenuVm : UIElementVm
    {
        private string _stationId;
        private readonly SelectionService _selectionService;
        
        public override Enums.UIElements Key => Enums.UIElements.GameWindowStationMenu;
        
        public GameWindowStationMenuVm(Data.Proxy.Station proxy, SelectionService selectionService)
        {
            _selectionService = selectionService;
            _stationId = proxy.ID;
        }

        public void MarkStationUnselected()
        {
            _selectionService.MarkUnselected(_stationId);
        }
    }
}