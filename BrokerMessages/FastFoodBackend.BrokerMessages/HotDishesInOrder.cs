namespace FastFoodBackend.BrokerMessages
{
    public class HotDishesInOrder
    {
        public List<HotDish> HotDishes { get; set; }
        public Guid OrderId { get; set; }

        public HotDishesInOrder(Guid orderId, List<HotDish> hotDishes)
        {
            OrderId = orderId;
            HotDishes = hotDishes;
        }
    }
}
