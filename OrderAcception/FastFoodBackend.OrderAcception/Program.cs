using FastFoodBackend.OrderAcception.Common;
using FastFoodBackend.OrderAcception.Infrastructure;

namespace FastFoodBackend.OrderAcception
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
                "http://localhost:8083",
                "http://" + LocalIp + ":8083");

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
