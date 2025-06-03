using FastFoodBackend.Contracts.CacheModels;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories
{
    public interface IOrderInCacheRepository
    {
        Task<OrderInCache> GetOrderAsync(Guid orderId);
        Task<OrderInCache> GetOrderAsync(Guid orderId, HashSet<string> itemsNames);
        Task SetOrderItemClosedAsync(OrderInCache orderInCache, string item);
        Task SetOrderItemClosedAsync(OrderInCache orderInCache, HashSet<string> items);
    }
}
