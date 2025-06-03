using FastFoodBackend.Contracts.ApiAndBrokersModels;
using FastFoodBackend.Contracts.BrokerModels;

using FastFoodBackend.OrderAcception.Application.Abstract.Repositories;
using FastFoodBackend.OrderAcception.Application.Abstract.Services;

using MassTransit;
using Microsoft.Extensions.Logging;

namespace FastFoodBackend.OrderAcception.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPublishEndpoint _messageBroker;
        private readonly IOrderInCacheRepository _orderInCacheRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService
        (
            IPublishEndpoint publishEndpoint,
            IOrderInCacheRepository orderInCacheRepository,
            ILogger<OrderService> logger)
        {
            _messageBroker = publishEndpoint;
            _orderInCacheRepository = orderInCacheRepository;
            _logger = logger;
        }

        public async Task AcceptOrderAsync(Order order)
        {
            _logger.LogInformation($"Accept order operation begin, order: {order.ToString()}");

            await _orderInCacheRepository.SaveOrderAsync(order); // сохраняем в Redis 

            if (order.HotDishes.Any()) // отправляем в очередь приготовления горячих блюд
            {
                await _messageBroker.Publish(new HotDishesInOrder(order.Id, order.HotDishes));
            }

            if (order.ColdDishes.Any()) // отправляем в очередь приготовления холодных блюд
            {
                await _messageBroker.Publish(new ColdDishesInOrder(order.Id, order.ColdDishes));
            }

            if (order.Drinks.Any()) // отправляем в очередь приготовления напитков
            {
                await _messageBroker.Publish(new DrinksInOrder(order.Id, order.Drinks));
            }
        }
    }
}
