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
                case "HOT_DISH":
                    ProcessPreparedHotDish(orderId, item);
                    break;
                case "COLD_DISH":
                    break;
                case "DRINK":
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
                var dish = JsonSerializer.Deserialize<HotDish>(hotDish.ToString());

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
