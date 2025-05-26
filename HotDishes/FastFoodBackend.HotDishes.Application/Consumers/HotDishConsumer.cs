using MassTransit;
using Microsoft.Extensions.Logging;
using FastFoodBackend.BrokerMessages;

namespace FastFoodBackend.HotDishes.Application.Consumers
{
    public class HotDishConsumer : IConsumer<HotDishesInOrder>
    {
        private readonly ILogger<HotDishConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public HotDishConsumer(ILogger<HotDishConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
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

                await _publishEndpoint.Publish(ItemPrepared.BuildHotDishPrepared(orderId, dish));
            }
        }
    }
}
