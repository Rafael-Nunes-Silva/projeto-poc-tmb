using api_poc_tmb.Data;
using api_poc_tmb.HealthChecks;
using api_poc_tmb.Services;
using api_poc_tmb.Services.Interfaces;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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

            var serviceBusConn = builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString");
            var serviceBusQueue = builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("QueueName");

            builder.Services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy("OK"), tags: new[] { "live" })
                .AddCheck("postgres", new PostgresHealthCheck(
                    builder.Configuration.GetConnectionString("DefaultConnection")!
                ), tags: new[] { "ready", "db" })
                .AddCheck("servicebus", new ServiceBusHealthCheck(
                    new ServiceBusAdministrationClient(serviceBusConn),
                    serviceBusQueue!
                ), tags: new[] { "ready", "queue" });

            builder.Services.AddScoped<IOpenAIService, OpenAIService>();
            builder.Services.AddScoped<ILLMSqlService, LLMSqlService>();

            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("live"),
                ResponseWriter = CustomHealthResponse.WriteResponse
            });

            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready"),
                ResponseWriter = CustomHealthResponse.WriteResponse
            });

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();
            }

            app.UseCors();

            app.MapControllers();

            app.Run();
        }
    }
}
