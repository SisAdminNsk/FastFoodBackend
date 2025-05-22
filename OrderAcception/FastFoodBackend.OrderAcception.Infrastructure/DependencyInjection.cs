using FastFoodBackend.BrokerMessages;
using FastFoodBackend.OrderAcception.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastFoodBackend.OrderAcception.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
        {
            AddRabbitMQ(services, configuration);
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
                    cfg.Message<Order>(x => x.SetEntityName("order-assembly-queue"));
                });
            });
        }
    }
}
