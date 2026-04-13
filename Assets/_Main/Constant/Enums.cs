namespace Constant
{
    public static class Enums
    {
        public enum Cameras
        {
            BootCam,
            MenuCam,
            GameCam,
        }
        
        public enum Entities
        {
            Station,
            Order
        }
        
        public enum Stations
        {
            Command,
            Mining,
            Extraction,
            Production,
            Ward,
            Energy,
            Logistic,
            Science,
            Yard,
            Resident
        }
        
        public enum UIElements
        {
            Container,
            Resource,
            Order
        }

        public enum Containers
        {
            //Root
            BootRoot,
            MenuRoot,
            GameRoot,
            
            //Window
            GameMenuWindow,
            
            //Popup
            
            //Token
        }
        
        public enum Resources
        {
            //Raws
            Ore,
            Biomass,
            Chemicals,
        
            //Materials
            Metals,
            Polymers,
            Minerals,
            Fuel,
            Fertilizers,
            Biomaterials,
        
            //Products
            Electronics,
            Composites,
            Food,
            Weapons,
            BuildingMaterials,
            Goods
        }
    }
}