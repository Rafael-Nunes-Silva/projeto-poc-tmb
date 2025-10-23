using api_poc_tmb.Data;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;

namespace api_poc_tmb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddSingleton<ServiceBusClient>(sp =>
            {
                var connString = builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString");
                return new ServiceBusClient(connString);
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
