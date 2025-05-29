using FastFoodBackend.Contracts.BrokerModels;

using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastFoodBackend.OrdersAssembly.Application.Consumers
{
    public class OrderAssemblyConsumer : IConsumer<OrderForAssembly>
    {
        private readonly ILogger<OrderAssemblyConsumer> _logger;

        public OrderAssemblyConsumer(ILogger<OrderAssemblyConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderForAssembly> context)
        {
            // сохранить пришедний заказ и начать отслеживать его выполнение

            _logger.LogInformation($"Order {context.Message.Order.Id} accept for processing");
        }
    }
}
