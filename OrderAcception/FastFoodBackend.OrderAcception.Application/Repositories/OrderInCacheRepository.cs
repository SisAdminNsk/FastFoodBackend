using FastFoodBackend.OrderAcception.Application.Abstract.Repositories;
using StackExchange.Redis;

namespace FastFoodBackend.OrderAcception.Application.Repositories
{
    public class OrderInCacheRepository : BaseDistributedCacheRepository, IOrderInCacheRepository
    {
        public OrderInCacheRepository(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer) 
        {
            
        }

        public async Task SaveOrderAsync(BrokerMessages.Order order)
        {
            var key = $"order:{order.Id}";

            var entriesList = new List<HashEntry>
            {
                new("total_items", order.ItemsCount()),
                new("total_cooked_items", 0)
            };

            var allItemsInOrder = order.GetAllItems();

            foreach(var item in allItemsInOrder)
            {
                entriesList.Add(new HashEntry(item.Name, false));
            }

            await _database.HashSetAsync(key, entriesList.ToArray());
        }
    }
}
