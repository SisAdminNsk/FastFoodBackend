using FastFoodBackend.Contracts.ApiModels;
using FastFoodBackend.Contracts.ReflectionUtils;
using System.Text.Json.Serialization;

namespace FastFoodBackend.Contracts.BrokerModels
{
    public partial class ItemPrepared
    {
        public Guid OrderId { get; private set; }
        public DishType DishType { get; private set; }
        public object Item { get; private set; }

        [JsonConstructor]
        private ItemPrepared(Guid orderId, DishType dishType, object item)
        {
            OrderId = orderId;
            DishType = dishType;
            Item = item;
        }

        public static ItemPrepared BuildDish<Dish>(Guid orderId, Dish dish) where Dish : IOrderItem
        {
            var dishType = DishTypeToDishClassMapper.GetDishTypeByDishClass(typeof(Dish));

            return new ItemPrepared(orderId, dishType, dish);
        }
    }
}
