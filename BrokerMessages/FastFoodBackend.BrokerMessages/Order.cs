namespace FastFoodBackend.BrokerMessages
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime AcceptionTime { get; set; }
        public DateTime? ReadyTime { get; set; }
        public List<Drink> Drinks { get; set; }
        public List<HotDish> HotDishes { get; set; }
        public List<ColdDish> ColdDishes { get; set; }
    }
}
