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
        public Enums.Resources TypeKey { get; }
        
        public ReactiveProperty<int> Amount { get; }

        public Resource(State.Resource origin)
        {
            Origin = origin;
            TypeKey = origin.ResourceType;
            
            Amount = new ReactiveProperty<int>(origin.Amount);
            Amount.Skip(1).Subscribe(amount => origin.Amount = amount);
        }
    }
}