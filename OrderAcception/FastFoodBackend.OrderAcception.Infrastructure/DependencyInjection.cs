using FastFoodBackend.BrokerMessages;
using FastFoodBackend.OrderAcception.Application.Abstract.Repositories;
using FastFoodBackend.OrderAcception.Application.Consumers;
using FastFoodBackend.OrderAcception.Application.Repositories;
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
            AddRedisCahce(services, configuration);

            AddRepositories(services);
        }
        public static void AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
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

        public static void AddRedisCahce(this IServiceCollection services, IConfiguration configuration) 
        {
            var redisConnection = configuration.GetRequiredSection("RedisConnection");
            var host = redisConnection["host"];

            services.AddSingleton<IConnectionMultiplexer>(options => ConnectionMultiplexer.Connect(host));
        }

        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IOrderInCacheRepository, OrderInCacheRepository>();
        }
    }
}
