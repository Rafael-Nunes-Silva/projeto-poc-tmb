using api_poc_tmb.Data;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using worker_orders;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var connString = builder.Configuration.GetSection("AzureServiceBus").GetValue<string>("ConnectionString");
    return new ServiceBusClient(connString);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
