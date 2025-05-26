namespace FastFoodBackend.BrokerMessages
{
    public class HotDish : IOrderItem
    {
        public string Name { get; set; }

        public HotDish(string name)
        {
            Name = name;
        }

        public HotDish() { } 
    }
}
