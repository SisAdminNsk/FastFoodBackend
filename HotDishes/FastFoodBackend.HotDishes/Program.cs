using FastFoodBackend.HotDishes.Consumers;
using MassTransit;
using System.Net;
using System.Net.Sockets;

namespace FastFoodBackend.HotDishes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var LocalIp = GetLocalIPAddress();

            builder.WebHost.UseUrls(
                "http://localhost:8084",
                "http://" + LocalIp + ":8084");

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<HotDishConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitConnection = builder.Configuration.GetRequiredSection("RabbitConnection");

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        static string GetLocalIPAddress()
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;

                if (endPoint != null)
                {
                    return endPoint.Address.ToString();
                }
                else
                {
                    return "127.0.0.1";
                }
            }
        }
    }
}
