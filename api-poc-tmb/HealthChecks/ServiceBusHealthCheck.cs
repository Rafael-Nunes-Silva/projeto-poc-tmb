using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace api_poc_tmb.HealthChecks
{
    public class ServiceBusHealthCheck : IHealthCheck
    {
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly string _queueName;
        private readonly int _timeoutMs;

        public ServiceBusHealthCheck(ServiceBusAdministrationClient adminClient, string queueName, int timeoutMs = 3000)
        {
            _adminClient = adminClient ?? throw new ArgumentNullException(nameof(adminClient));
            _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
            _timeoutMs = timeoutMs;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(_timeoutMs);

                var props = await _adminClient.GetQueueRuntimePropertiesAsync(_queueName, cts.Token);
                return HealthCheckResult.Healthy("Azure Service Bus disponivel");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Azure Service Bus indisponivel", ex);
            }
        }
    }
}
