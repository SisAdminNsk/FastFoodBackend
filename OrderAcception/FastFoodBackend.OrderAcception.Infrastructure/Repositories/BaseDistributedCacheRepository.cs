using StackExchange.Redis;

namespace FastFoodBackend.OrderAcception.Infrastructure.Repositories
{
    internal abstract class BaseDistributedCacheRepository
    {
        protected readonly IDatabase _database;

        public BaseDistributedCacheRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
    }
}
