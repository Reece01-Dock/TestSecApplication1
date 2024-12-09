using System.Net;

namespace TestSecApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var publicIp = await GetPublicIpAddressAsync();

            var builder = WebApplication.CreateBuilder(args);

            // Configure Kestrel to listen on the public IP and port
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Parse(publicIp), 5000); // Replace 8080 with your desired port
            });

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }

        private static async Task<string> GetPublicIpAddressAsync()
        {
            using var client = new HttpClient();
            try
            {
                // Fetch the public IP address from an external API
                var ip = await client.GetStringAsync("https://api.ipify.org");
                return ip.Trim();
            }
            catch
            {
                // Default to 0.0.0.0 if public IP cannot be retrieved
                return "0.0.0.0";
            }
        }
    }
}
