using Daybreak.API.Models;
using Daybreak.API.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;
using ZLinq;

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
                    Entries = report.Entries.AsValueEnumerable().Select(e => (e, new HealthCheckEntryResponse
                    {
                        Status = e.Value.Status,
                        Data = e.Value.Data.AsValueEnumerable().Where(kvp => kvp.Value is JsonElement).ToDictionary(kvp => kvp.Key, kvp => (JsonElement)kvp.Value),
                        Description = e.Value.Description ?? string.Empty,
                        Tags = [.. e.Value.Tags]
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
