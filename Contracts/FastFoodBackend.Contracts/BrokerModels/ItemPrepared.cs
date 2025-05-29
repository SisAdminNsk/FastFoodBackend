using FastFoodBackend.Contracts.ApiModels;
using System.Text.Json.Serialization;

namespace FastFoodBackend.Contracts.BrokerModels
{
    public partial class ItemPrepared
    {
        public Guid OrderId { get; private set; }
        public string Type { get; private set; }
        public object Item { get; private set; }

        [JsonConstructor]
        private ItemPrepared(Guid orderId, string type, object item)
        {
            OrderId = orderId;
            Type = type;
            Item = item;
        }
        public static ItemPrepared BuildHotDishPrepared(Guid orderId, object item)
        {
            return new ItemPrepared(orderId, DishesTypes.HotDish, item);
        }

        public static ItemPrepared BuildColdDishPrepared(Guid orderId, object item)
        {
            return new ItemPrepared(orderId, DishesTypes.ColdDish, item);
        }

        public static ItemPrepared BuildDrinkPrepared(Guid orderId, object item)
        {
            return new ItemPrepared(orderId, DishesTypes.Drink, item);
        }
    }
}
