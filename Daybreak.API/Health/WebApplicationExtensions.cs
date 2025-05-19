using Daybreak.API.Models;
using Daybreak.API.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Daybreak.API.Health;

public static class WebApplicationExtensions
{
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (ctx, report) =>
            {
                ctx.Response.ContentType = "application/json";
                var response = new HealthCheckResponse
                {
                    Status = report.Status,
                    TotalDuration = report.TotalDuration,
                    Entries = report.Entries.Select(e => (e, new HealthCheckEntryResponse
                    {
                        Status = e.Value.Status,
                        Data = e.Value.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString() ?? string.Empty),
                        Description = e.Value.Description ?? string.Empty,
                        Tags = e.Value.Tags.ToList()
                    })).ToDictionary(e => e.e.Key, e => e.Item2)
                };

                var serialized = JsonSerializer.Serialize(
                    response,
                    ApiJsonSerializerContext.Default.HealthCheckResponse);
                await ctx.Response.WriteAsync(serialized);
            }
        });
        return app;
    }
}
