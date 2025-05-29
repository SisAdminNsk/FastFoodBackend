using FastFoodBackend.Contracts.ApiAndBrokersModels;

namespace FastFoodBackend.OrderAcception.Application.Abstract.Repositories
{
    public interface IOrderInCacheRepository 
    {
        Task SaveOrderAsync(Order order);
    }
}
