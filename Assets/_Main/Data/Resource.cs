using Constant;
using R3;

namespace Data.State
{
    public class Resource
    {
        public Enums.Resources ResourceType { get; set; }
        public int Amount { get; set; }
    }
}

namespace Data.Proxy
{
    public class Resource
    {
        public State.Resource Origin { get; }
        public Enums.Resources ResourceType => Origin.ResourceType;
        
        public ReactiveProperty<int> Amount { get; }

        public Resource(State.Resource origin)
        {
            Origin = origin;
            
            Amount = new ReactiveProperty<int>(origin.Amount);
            Amount.Skip(1).Subscribe(amount => origin.Amount = amount);
        }
    }
}