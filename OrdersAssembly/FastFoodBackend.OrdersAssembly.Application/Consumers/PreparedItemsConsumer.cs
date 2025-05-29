using FastFoodBackend.Contracts.BrokerModels;
using FastFoodBackend.Contracts.ApiModels;

using MassTransit;
using System.Text.Json;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories;

namespace FastFoodBackend.OrdersAssembly.Application.Consumers
{
    public class PreparedItemsConsumer : IConsumer<ItemPrepared>
    {
        private readonly IOrderInCacheRepository _orderInCacheRepository;

        public PreparedItemsConsumer(IOrderInCacheRepository orderInCacheRepository)
        {
            _orderInCacheRepository = orderInCacheRepository;
        }

        public async Task Consume(ConsumeContext<ItemPrepared> context)
        {
            var message = context.Message;

            await ProcessPreparedItems(message.OrderId, message.Type, message.Item);
        }
        private async Task ProcessPreparedItems(Guid orderId, string itemType, object item)
        {
            Console.WriteLine($"Received prepared item for OrderId: {orderId}");

            Console.WriteLine($"Item type is: {itemType}");

            switch (itemType)
            {
                case DishesTypes.HotDish:
                    ProcessPreparedDish<HotDish>(orderId, item);
                    break;
                case DishesTypes.ColdDish:
                    ProcessPreparedDish<ColdDish>(orderId, item);
                    break;
                case DishesTypes.Drink:
                    ProcessPreparedDish<Drink>(orderId, item);
                    break;
                default:
                    Console.WriteLine("Неизвестный тип блюда");
                    break;
            }
        }

        private void ProcessPreparedDish<Dish>(Guid orderId, object hotDish) where Dish : IOrderItem
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                var dish = JsonSerializer.Deserialize<Dish>(hotDish.ToString(), options);

                Console.WriteLine($"{dish.Name} для заказа: ${orderId} добавлено в итоговую позицию");

                var fields = new HashSet<string>() {dish.Name};

                //var order1 = _orderInCacheRepository.GetOrderInCacheRecordAsync(orderId).Result;
                //var order2 = _orderInCacheRepository.GetOrderInCacheRecordAsync(orderId, fields).Result;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Невозможно корректно преобразовать тип объекта: {ex.Message}");
                return;
            }
        }
    }
}
