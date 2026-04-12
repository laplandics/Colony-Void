using Constant;

namespace Data.State
{
    public class Station : Entity
    {
        public Enums.Stations StationType { get; set; }
    }
}

namespace Data.Proxy
{
    public class Station : Entity
    {
        public Enums.Stations StationType { get; }
        
        public Station(State.Station origin) : base(origin)
        {
            StationType = origin.StationType;
        }
    }
}