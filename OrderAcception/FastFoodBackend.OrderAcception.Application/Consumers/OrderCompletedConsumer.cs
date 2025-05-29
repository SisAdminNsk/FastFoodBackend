using FastFoodBackend.Contracts.BrokerModels;

using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastFoodBackend.OrderAcception.Application.Consumers
{
    public class OrderCompletedConsumer : IConsumer<OrderCompleted>
    {
        private readonly ILogger<OrderCompletedConsumer> _logger;

        public OrderCompletedConsumer(ILogger<OrderCompletedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCompleted> context)
        {
            _logger.LogInformation($"Order {context.Message.Id} completed and ready for pickup");
        }
    }
}
