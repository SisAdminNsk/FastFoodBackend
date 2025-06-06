using FastFoodBackend.Contracts.BrokerModels;
using FastFoodBackend.Contracts.CacheModels;

using FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;

using Microsoft.Extensions.Logging;

namespace FastFoodBackend.OrdersAssembly.Application.Services
{
    public class OrdersTrackerService : IOrdersTrackerService, IDisposable
    {
        public event Func<object, OrderCompleted, Task>? OrderIsReadyAsync;

        private readonly ILogger<OrdersTrackerService> _logger;
        private readonly IOrderInCacheRepository _orderInCacheRepository;
        private readonly IDishConverterService _dishConverterService;

        public OrdersTrackerService(
            IOrderInCacheRepository orderInCacheRepository,
            IDishConverterService dishConverterService,
            ILogger<OrdersTrackerService> logger)
        {
            _orderInCacheRepository = orderInCacheRepository;
            _dishConverterService = dishConverterService;
            _logger = logger;
        }

        public async Task OnPreparedItemReceived(ItemPrepared preparedItem)
        {
            var orderId = preparedItem.OrderId;

            var order = await _orderInCacheRepository.GetOrderAsync(orderId);

            if (IsOrderCompleted(order))
            {
                _logger.LogInformation($"Заказ {orderId} собран");

                var preparedItems = order.ItemsInOrderStatuses.Select(k => k.Key).ToList();

                await OnOrderReady(new OrderCompleted(orderId, preparedItems));
            }
            else
            {
                var orderItem = _dishConverterService.ConvertDish(preparedItem.DishType, preparedItem.Item);

                await _orderInCacheRepository.SetOrderItemClosedAsync(order, orderItem.Name);

                _logger.LogInformation($"Позиция {orderItem.Name} внутри заказа {orderId} закрыта");
            }
        }

        private async Task OnOrderReady(OrderCompleted args)
        {
            if (OrderIsReadyAsync != null)
            {
                var handlers = OrderIsReadyAsync.GetInvocationList()
                    .Cast<Func<object, OrderCompleted, Task>>();

                var tasks = handlers.Select(h => h(this, args));

                await Task.WhenAll(tasks);
            }
        }

        private bool IsOrderCompleted(OrderInCache order)
        {
            if(order.TotalItemsInOrder == order.TotalCookedItems + 1)
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            OrderIsReadyAsync = null;
        }
    }
}
