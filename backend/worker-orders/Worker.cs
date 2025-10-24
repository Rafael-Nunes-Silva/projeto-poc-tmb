using Azure.Messaging.ServiceBus;
using api_poc_tmb.Data;
using api_poc_tmb.Models;
using System.Text.Json;

namespace worker_orders;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusClient _busClient;
    private readonly string _queueName = "orders-queue";

    public Worker(
        ILogger<Worker> logger,
        IServiceProvider serviceProvider,
        ServiceBusClient busClient
        )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _busClient = busClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var processor = _busClient.CreateProcessor(_queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        });

        processor.ProcessMessageAsync += ProcessMessageAsync;
        processor.ProcessErrorAsync += ProcessErrorAsync;

        _logger.LogInformation("Worker iniciado e esperando mensagens: {time}", DateTimeOffset.Now);

        await processor.StartProcessingAsync(stoppingToken);

        await Task.Delay(-1, stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var body = message.Body.ToString();
        _logger.LogInformation("Mensagem recebida: {MessageBody}", body);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var jsonDoc = JsonDocument.Parse(body);
            var orderId = jsonDoc.RootElement.GetProperty("OrderId").GetInt32();

            var order = await db.orders.FindAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Pedido {OrderId} nao encontrado. Ignorando mensagem.", orderId);
                await args.CompleteMessageAsync(message);
                return;
            }

            // So processa se estiver Pendente
            if (order.Status != EOrderStatus.Pendente)
            {
                _logger.LogInformation("Pedido {OrderId} ja processado ({Status}). Ignorando.", orderId, order.Status);
                await args.CompleteMessageAsync(message);
                return;
            }

            order.Status = EOrderStatus.Processando;
            await db.SaveChangesAsync();

            _logger.LogInformation("Pedido {OrderId} em processamento...", orderId);

            // Simula processamento de 5 segundos
            await Task.Delay(TimeSpan.FromSeconds(5));

            order.Status = EOrderStatus.Finalizado;
            await db.SaveChangesAsync();

            _logger.LogInformation("Pedido {OrderId} finalizado.", orderId);

            await args.CompleteMessageAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro processando mensagem {MessageId}", message.MessageId);
            await args.AbandonMessageAsync(message);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Erro na fila: {ErrorSource}", args.ErrorSource);
        return Task.CompletedTask;
    }
}
