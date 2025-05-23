using FastFoodBackend.BrokerMessages;
using FastFoodBackend.OrdersAssembly.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastFoodBackend.OrdersAssembly.Infrastructure
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
                x.AddConsumer<OrderAssemblyConsumer>();
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

                    // Подписка на очередь только что поступивших заказов
                    cfg.ReceiveEndpoint("order-assembly-queue", e =>
                    {
                        e.ConfigureConsumer<OrderAssemblyConsumer>(context);
                    });

                    // Общая очередь для всех приготовленных позиций
                    cfg.ReceiveEndpoint("prepared-items-queue", e =>
                    {
                        e.ConfigureConsumer<PreparedItemsConsumer>(context);
                    });

                    cfg.Message<OrderCompleted>(x => x.SetEntityName("order-completed-queue"));
                });
            });
        }
    }
}
