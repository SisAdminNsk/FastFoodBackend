using FastFoodBackend.BrokerMessages;
using FastFoodBackend.OrderAcception.Application.Abstract.Repositories;
using FastFoodBackend.OrderAcception.Controllers.AuthenticationService.Api.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodBackend.OrderAcception.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class OrdersController : BaseApiController
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOrderInCacheRepository _orderInCacheRepository;

        public OrdersController(
            ILogger<OrdersController> logger,
            IPublishEndpoint publishEndpoint,
            IOrderInCacheRepository orderInCacheRepository) : base(logger)
        {
            _publishEndpoint = publishEndpoint;
            _orderInCacheRepository = orderInCacheRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            Console.WriteLine($"Заказ создан с id ${order.Id}");

            await _publishEndpoint.Publish(new OrderForAssembly(order));

            await _orderInCacheRepository.SaveOrderAsync(order);

            if (order.HotDishes.Any())
            {
                await _publishEndpoint.Publish(new HotDishesInOrder(order.Id, order.HotDishes));
            }

            if(order.ColdDishes.Any())
            {
                await _publishEndpoint.Publish(new ColdDishesInOrder(order.Id, order.ColdDishes));
            }

            if (order.Drinks.Any())
            {
                await _publishEndpoint.Publish(new DrinksInOrder(order.Id, order.Drinks));
            }

            return Ok();
        }
    }
}
