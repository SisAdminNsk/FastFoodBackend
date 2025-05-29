using FastFoodBackend.Contracts.ApiAndBrokersModels;

using FastFoodBackend.OrderAcception.Application.Abstract.Services;
using FastFoodBackend.OrderAcception.Controllers.AuthenticationService.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FastFoodBackend.OrderAcception.Controllers
{
    [ApiController]
    [Route("v1/orders")]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService) : base(logger)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.AcceptOrderAsync(order);

            Console.WriteLine($"Заказ создан с id ${order.Id}");

            return Ok(order.Id);
        }
    }
}
