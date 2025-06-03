using FastFoodBackend.Contracts.CacheModels;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories;
using StackExchange.Redis;

namespace FastFoodBackend.OrdersAssembly.Infrastructure.Repositories
{
    public class OrderInCacheRepository : BaseDistributedCacheRepository, IOrderInCacheRepository
    {
        public OrderInCacheRepository(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer) 
        {
            
        }

        public async Task<OrderInCache> GetOrderAsync(Guid orderId)
        {
            var hashEntries = await _database.HashGetAllAsync(BuildKey(orderId));

            if (hashEntries == null || hashEntries.Length == 0)
                throw new KeyNotFoundException($"Order {orderId} not found in Redis");

            int totalItems = 0;
            int totalCookedItems = 0;
            var itemInOrderStatuses = new Dictionary<string, bool>();

            foreach (var entry in hashEntries)
            {
                string fieldName = entry.Name;
                RedisValue fieldValue = entry.Value;

                if (fieldName == "total_items")
                {
                    totalItems = (int)fieldValue;
                }
                else if (fieldName == "total_cooked_items")
                {
                    totalCookedItems = (int)fieldValue;
                }
                else
                {
                    // Остальные поля считаем флагами готовности блюд (1 = true, 0 = false)
                    bool isReady = (int)fieldValue == 1;
                    itemInOrderStatuses.Add(fieldName, isReady);
                }
            }

            return new OrderInCache(orderId, totalItems, totalCookedItems, itemInOrderStatuses);
        }

        public async Task<OrderInCache> GetOrderAsync(Guid orderId, HashSet<string> itemsNames)
        {
            var fields = new List<RedisValue>() {"total_items", "total_cooked_items" };

            foreach(var itemName in itemsNames)
            {
                fields.Add(new RedisValue(itemName));
            }

            var values = await _database.HashGetAsync(BuildKey(orderId), fields.ToArray());

            int totalItems = (int)values[0];
            int totalCookedItems = (int)values[1];

            var itemInOrderStatuses = new Dictionary<string, bool>();

            for (int i = 0; i < itemsNames.Count; i++)
            {
                string itemName = itemsNames.ElementAt(i);
                bool isReady = (int)values[2 + i] == 1;  // 1 = true, 0 = false
                itemInOrderStatuses.Add(itemName, isReady);
            }

            return new OrderInCache(orderId, totalItems, totalCookedItems, itemInOrderStatuses);
        }

        public async Task SetOrderItemClosedAsync(OrderInCache order, string item)
        {
            var fieldsToUpdate = new List<HashEntry>()
            {
                new HashEntry("total_cooked_items", order.TotalCookedItems + 1),
                new HashEntry(item, 1)
            };

            await _database.HashSetAsync(BuildKey(order.OrderId), fieldsToUpdate.ToArray());
        }

        public async Task SetOrderItemClosedAsync(OrderInCache order, HashSet<string> items)
        {
            var closedItemsCount = items.Count;

            var fieldsToUpdate = new List<HashEntry>()
            {
                new HashEntry("total_cooked_items", order.TotalCookedItems + closedItemsCount)
            };

            foreach(var item in items)
            {
                fieldsToUpdate.Add(new HashEntry(item, 1));
            }

            await _database.HashSetAsync(BuildKey(order.OrderId), fieldsToUpdate.ToArray());
        }

        private string BuildKey(Guid orderId)
        {
            return $"order:{orderId}";
        }
    }
}
