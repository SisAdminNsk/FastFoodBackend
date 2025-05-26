using FastFoodBackend.BrokerMessages;
using MassTransit;
using System.Text.Json;

namespace FastFoodBackend.OrdersAssembly.Application.Consumers
{
    public class PreparedItemsConsumer : IConsumer<ItemPrepared>
    {
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
                    ProcessPreparedHotDish(orderId, item);
                    break;
                case DishesTypes.ColdDish:
                    break;
                case DishesTypes.Drink:
                    break;
                default:
                    Console.WriteLine("Неизвестный тип блюда");
                    break;
            }
        }

        private void ProcessPreparedHotDish(Guid orderId, object hotDish)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                var dish = JsonSerializer.Deserialize<HotDish>(hotDish.ToString(), options);

                Console.WriteLine($"${dish.Name} для заказа: ${orderId} добавлено в итоговую позицию");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Невозможно корректно преобразовать тип объекта: {ex.Message}");
                return;
            }
        }
    }
}
