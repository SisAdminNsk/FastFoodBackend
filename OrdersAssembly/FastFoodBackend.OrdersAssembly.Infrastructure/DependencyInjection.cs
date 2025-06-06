using FastFoodBackend.OrdersAssembly.Application.Abstract.Repositories;
using FastFoodBackend.OrdersAssembly.Application.Abstract.Services;
using FastFoodBackend.OrdersAssembly.Application.Consumers;
using FastFoodBackend.OrdersAssembly.Application.Services;
using FastFoodBackend.OrdersAssembly.Infrastructure.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FastFoodBackend.OrdersAssembly.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRabbitMQ(services, configuration);
            AddRedisCache(services, configuration);

            AddRepositories(services);
            AddServices(services);
        }
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<PreparedItemsConsumer>(); 

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitConnection = configuration.GetRequiredSection("RabbitConnection");

                    var rabbitHost = rabbitConnection["host"];

                    var rabbitPassword = rabbitConnection["password"];
                    var rabbitUsername = rabbitConnection["name"];

                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username(rabbitUsername);
                        h.Password(rabbitPassword);
                    });

                    // Общая очередь для всех приготовленных позиций
                    cfg.ReceiveEndpoint("prepared-items-queue", e =>
                    {
                        e.ConfigureConsumer<PreparedItemsConsumer>(context);
                    });
                });
            });
        }
        private static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnection = configuration.GetRequiredSection("RedisConnection");
            var host = redisConnection["host"];

            services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(host));
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderInCacheRepository, OrderInCacheRepository>();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IOrdersTrackerService, OrdersTrackerService>();
            services.AddTransient<IDishConverterService, DishConverterService>();
        }
    }
}
