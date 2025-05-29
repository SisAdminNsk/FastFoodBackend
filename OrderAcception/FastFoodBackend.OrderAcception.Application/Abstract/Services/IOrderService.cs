using FastFoodBackend.Contracts.ApiAndBrokersModels;

namespace FastFoodBackend.OrderAcception.Application.Abstract.Services
{
    public interface IOrderService
    {
        Task AcceptOrderAsync(Order order);
    }
}
