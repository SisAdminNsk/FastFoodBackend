
using FastFoodBackend.OrdersAssembly.Common;
using FastFoodBackend.OrdersAssembly.Infrastructure;

namespace FastFoodBackend.OrdersAssembly
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

            builder.Services.AddInfrastructure(builder.Configuration); // Добавляем инфраструктуру

            string LocalIp = IPAddressUtils.GetLocalIPAddress();

            builder.WebHost.UseUrls(
                "http://localhost:8085",
                "http://" + LocalIp + ":8085");

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
    }
}
