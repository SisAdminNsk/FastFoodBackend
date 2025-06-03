using FastFoodBackend.Contracts.ApiModels;
using FastFoodBackend.Contracts.BrokerModels;
using FastFoodBackend.Contracts.CacheModels;

using FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;

using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FastFoodBackend.OrdersAssembly.Application.Services
{
    public class OrdersTrackerService : IOrdersTrackerService, IDisposable
    {
        public event Func<object, OrderCompleted, Task>? OrderIsReadyAsync;

        private readonly ILogger<OrdersTrackerService> _logger;
        private readonly IOrderInCacheRepository _orderInCacheRepository;

        public OrdersTrackerService(
            IOrderInCacheRepository orderInCacheRepository,
            ILogger<OrdersTrackerService> logger)
        {
            _orderInCacheRepository = orderInCacheRepository;
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
                var orderItem = GetOrderItem(preparedItem.Type, preparedItem.Item);

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

        private IOrderItem GetOrderItem(string itemType, object serializedDish)
        {
            switch (itemType)
            {
                case DishesTypes.HotDish:
                    return ConvertDish<HotDish>(serializedDish);
                case DishesTypes.ColdDish:
                    return ConvertDish<ColdDish>(serializedDish);
                case DishesTypes.Drink:
                    return ConvertDish<Drink>(serializedDish);
                default:
                    throw new NotSupportedException($"Неизвестный тип блюда: {itemType}");
            }
        }
        private Dish? ConvertDish<Dish>(object serializedDish) where Dish : IOrderItem
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = true,
                    PropertyNameCaseInsensitive = true
                };

                var dish = JsonSerializer.Deserialize<Dish>(serializedDish.ToString(), options);

                return dish;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Невозможно корректно преобразовать тип объекта: {ex.Message}");

                throw;
            }
        }

        public void Dispose()
        {
            OrderIsReadyAsync = null;
        }
    }
}
