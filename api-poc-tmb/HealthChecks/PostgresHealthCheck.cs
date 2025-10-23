using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace api_poc_tmb.HealthChecks
{
    public class PostgresHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly int _timeoutMs;

        public PostgresHealthCheck(string connectionString, int timeoutMs = 3000)
        {
            _connectionString = connectionString;
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

                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync(cts.Token);

                await using var cmd = new NpgsqlCommand("SELECT 1", conn);
                await cmd.ExecuteScalarAsync(cts.Token);

                return HealthCheckResult.Healthy("Postgres OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Postgres indisponivel", ex);
            }
        }
    }
}
