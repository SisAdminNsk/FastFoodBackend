using FastFoodBackend.HotDishes.Application.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FastFoodBackend.HotDishes.Infrastructure
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
                x.AddConsumer<HotDishConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitConnection = configuration.GetRequiredSection("RabbitConnection");

                    var rabbitHost = rabbitConnection["host"];

                    var rabbitPassword = rabbitConnection["password"];
                    var rabbitUsername = rabbitConnection["name"];

                    cfg.Host(rabbitHost, "/", h =>
                    {
                        h.Username(rabbitPassword);
                        h.Password(rabbitUsername);
                    });

                    // Подписываемся на очередь hot-dishes-queue
                    cfg.ReceiveEndpoint("hot-dishes-queue", e =>
                    {
                        e.ConfigureConsumer<HotDishConsumer>(context);
                    });
                });
            });
        }
    }
}
