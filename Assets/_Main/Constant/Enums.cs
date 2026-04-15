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
            //Root
            BootRoot = 10,
            MenuRoot = 11,
            GameRoot = 12,
            
            //Screen
            GameScreenMain = 20,
            BootScreenMain = 21,
            MenuScreenMain = 22,
            
            //Token
            
            //Window
            GameWindowGameMenu = 40
            
        }
        
        public enum Resources
        {
            //Raws
            Ore,
            Biomass,
            Chemical,
        
            //Materials
            Metal,
            Polymer,
            Mineral,
            Fuel,
            Fertilizer,
            Biomaterial,
        
            //Products
            Electronic,
            Composite,
            Food,
            Weapon,
            BuildingMaterial,
            GeneralGood
        }
    }
}