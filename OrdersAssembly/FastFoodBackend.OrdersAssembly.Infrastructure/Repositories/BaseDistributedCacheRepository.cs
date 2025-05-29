using StackExchange.Redis;

namespace FastFoodBackend.OrdersAssembly.Infrastructure.Repositories
{
    public abstract class BaseDistributedCacheRepository
    {
        protected readonly IDatabase _database;

        public BaseDistributedCacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
    }
}
