using FastFoodBackend.Contracts.BrokerModels;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;

using MassTransit;

namespace FastFoodBackend.OrdersAssembly.Application.Consumers
{
    public class PreparedItemsConsumer : IConsumer<ItemPrepared>, IDisposable
    {
        private readonly IOrdersTrackerService _orderTrackerService;
        private readonly IPublishEndpoint _messageBroker;

        public PreparedItemsConsumer(
            IOrdersTrackerService orderTrackerService,
            IPublishEndpoint publishEndpoint)
        {
            _orderTrackerService = orderTrackerService;
            _messageBroker = publishEndpoint;

            _orderTrackerService.OrderIsReadyAsync += OnOrderIsReady;
        }

        public async Task Consume(ConsumeContext<ItemPrepared> context)
        {
            await _orderTrackerService.OnPreparedItemReceived(context.Message);
        }

        public void Dispose()
        {
            _orderTrackerService.OrderIsReadyAsync -= OnOrderIsReady;
            _orderTrackerService?.Dispose();
        }

        private async Task OnOrderIsReady(object? sender, OrderCompleted e)
        {
            await _messageBroker.Publish(e);
        }
    }
}
