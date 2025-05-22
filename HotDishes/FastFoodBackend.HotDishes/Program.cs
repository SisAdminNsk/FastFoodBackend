using FastFoodBackend.HotDishes.Common;
using FastFoodBackend.HotDishes.Infrastructure;

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

            builder.Services.AddInfrastructure(builder.Configuration); // ��������� ��������������

            var LocalIp = IPAddressUtils.GetLocalIPAddress();

            builder.WebHost.UseUrls(
                "http://localhost:8084",
                "http://" + LocalIp + ":8084");

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
