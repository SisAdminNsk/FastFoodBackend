using System.Net.Sockets;
using System.Net;

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

            string LocalIp = GetLocalIPAddress();

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
