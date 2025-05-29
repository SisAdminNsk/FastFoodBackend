using FastFoodBackend.Contracts.ApiModels;

namespace FastFoodBackend.Contracts.BrokerModels
{
    public class ColdDishesInOrder
    {
        public List<ColdDish> ColdDishes { get; set; }
        public Guid OrderId { get; set; }
        public ColdDishesInOrder(Guid id, List<ColdDish> coldDishes)
        {
            OrderId = id;
            ColdDishes = coldDishes;
        }
    }
}
