using Constant;
using R3;

namespace Data.State
{
    public class Resource : UIElement
    {
        public Enums.Resources ResourceType { get; set; }
        public int Amount { get; set; }
    }
}

namespace Data.Proxy
{
    public class Resource : UIElement
    {
        public Enums.Resources ResourceType { get; }
        
        public ReactiveProperty<int> Amount { get; }

        public Resource(State.Resource origin) : base(origin)
        {
            ResourceType = origin.ResourceType;
            
            Amount = new ReactiveProperty<int>(origin.Amount);
            Amount.Skip(1).Subscribe(amount => origin.Amount = amount);
        }
    }
}