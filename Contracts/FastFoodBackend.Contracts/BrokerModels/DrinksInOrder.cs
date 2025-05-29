using FastFoodBackend.Contracts.ApiModels;

namespace FastFoodBackend.Contracts.BrokerModels
{
    public class DrinksInOrder
    {
        public List<Drink> Drinks { get; set; }
        public Guid OrderId { get; set; }

        public DrinksInOrder(Guid orderId, List<Drink> drinks)
        {
            OrderId = orderId;
            Drinks = drinks;
        }
    }
}
