using FastFoodBackend.BrokerMessages;
using MassTransit;

namespace FastFoodBackend.HotDishes.Consumers
{
    public class HotDishConsumer : IConsumer<HotDishesInOrder>
    {
        private readonly ILogger<HotDishConsumer> _logger;

        public HotDishConsumer(ILogger<HotDishConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<HotDishesInOrder> context)
        {
            var dishes = context.Message.HotDishes;
            var orderId = context.Message.OrderId;

            foreach (var dish in dishes)
            {
                _logger.LogInformation($"Приготовление горячего блюда {dish.Name} для заказа {orderId}");

                // Симуляция приготовления
                await Task.Delay(TimeSpan.FromSeconds(2));


                _logger.LogInformation($"Горячее блюдо {dish.Name} готово (заказ {orderId})");
            }
        }
    }
}
