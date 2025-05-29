using FastFoodBackend.Contracts.CacheModels;

namespace FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories
{
    public interface IOrderInCacheRepository
    {
        Task<OrderInCache> GetOrderInCacheRecordAsync(Guid orderId);
        Task<OrderInCache> GetOrderInCacheRecordAsync(Guid orderId, HashSet<string> itemsNames);
    }
}
