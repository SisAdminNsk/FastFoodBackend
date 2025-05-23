using FastFoodBackend.BrokerMessages;
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

        public OrdersController(ILogger<OrdersController> logger, IPublishEndpoint publishEndpoint) : base(logger)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            Console.WriteLine($"Заказ создан с id ${order.Id}");

            await _publishEndpoint.Publish(new OrderForAssembly(order));

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
