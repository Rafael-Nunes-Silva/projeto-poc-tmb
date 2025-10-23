using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace api_poc_tmb.HealthChecks
{
    public static class CustomHealthResponse
    {
        public static async Task WriteResponse(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.StatusCode = report.Status == HealthStatus.Healthy ? 200 : 503;

            var result = new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.ToString(),
                    data = e.Value.Data
                })
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
