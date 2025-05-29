using FastFoodBackend.Contracts.BrokerModels;

using FastFoodBackend.OrderAcception.Application.Abstract.Repositories;
using FastFoodBackend.OrderAcception.Application.Abstract.Services;
using FastFoodBackend.OrderAcception.Application.Consumers;
using FastFoodBackend.OrderAcception.Application.Services;
using FastFoodBackend.OrderAcception.Infrastructure.Repositories;

using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FastFoodBackend.OrderAcception.Infrastructure
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
        private static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderCompletedConsumer>();

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

                    // Подписка на очередь завершенных заказов
                    cfg.ReceiveEndpoint("completed-orders-queue", e =>
                    {
                        e.ConfigureConsumer<OrderCompletedConsumer>(context);           
                    });

                    cfg.Message<HotDishesInOrder>(x => x.SetEntityName("hot-dishes-queue"));
                    cfg.Message<ColdDishesInOrder>(x => x.SetEntityName("cold-dishes-queue"));
                    cfg.Message<DrinksInOrder>(x => x.SetEntityName("drinks-queue"));
                    cfg.Message<OrderForAssembly>(x => x.SetEntityName("order-assembly-queue"));
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
            services.AddTransient<IOrderService, OrderService>();
        }   
    }
}
