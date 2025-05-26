using FastFoodBackend.BrokerMessages;

namespace FastFoodBackend.OrderAcception.Application.Abstract.Repositories
{
    public interface IOrderInCacheRepository 
    {
        Task SaveOrderAsync(Order order);
    }
}
