using StackExchange.Redis;

namespace FastFoodBackend.OrderAcception.Application.Abstract.Repositories
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
