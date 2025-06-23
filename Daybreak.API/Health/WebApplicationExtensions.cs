using Daybreak.API.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using ZLinq;

namespace Daybreak.API.Health;

public static class WebApplicationExtensions
{
    [RequiresUnreferencedCode("The handler uses a static method that gets referenced, so there's no unreferenced code to worry about")]
    [RequiresDynamicCode("The handler uses a static method, so there's no dynamic code to worry about")]
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapGet("/api/v1/rest/health", static async (HealthCheckService healthCheckService, CancellationToken cancellationToken) =>
        {
            var report = await healthCheckService.CheckHealthAsync(cancellationToken);

            var response = new HealthCheckResponse
            {
                ProcessId = Environment.ProcessId,
                Status = report.Status,
                TotalDuration = report.TotalDuration,
                Entries = report.Entries.ToDictionary(
                    e => e.Key,
                    e => new HealthCheckEntryResponse
                    {
                        Status = e.Value.Status,
                        Description = e.Value.Description ?? string.Empty,
                        Tags = [.. e.Value.Tags],
                        Data = e.Value.Data
                            .Where(kvp => kvp.Value is JsonElement je)
                            .ToDictionary(kvp => kvp.Key,
                                kvp => (JsonElement)kvp.Value)
                    })
            };

            return Results.Json(response);
        })
        .WithName("Health")
        .WithSummary("Service health")
        .WithDescription($"Current health status of the service. Returns a serialized object of type {nameof(HealthCheckResponse)}")
        .WithTags("Service")
        .Produces<HealthCheckResponse>(StatusCodes.Status200OK)
        .WithOpenApi();
        return app;
    }
}
