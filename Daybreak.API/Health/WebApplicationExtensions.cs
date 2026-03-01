using Daybreak.API.Models;
using Daybreak.API.Services.Interop;
using System.Diagnostics.CodeAnalysis;
using ZLinq;

namespace Daybreak.API.Health;

public static class WebApplicationExtensions
{
    [RequiresUnreferencedCode("The handler uses a static method that gets referenced, so there's no unreferenced code to worry about")]
    [RequiresDynamicCode("The handler uses a static method, so there's no dynamic code to worry about")]
    public static WebApplication UseHealthChecks(this WebApplication app)
    {
        app.MapGet("/api/v1/rest/health", static (IEnumerable<IInteropHealthService> services) =>
        {
            var entries = services
                .SelectMany(s => s.GetAddressHealth())
                .ToList();
            var response = new HealthCheckResponse(Environment.ProcessId, entries);
            return Results.Json(response);
        })
        .WithName("Health")
        .WithSummary("Interop hook health")
        .WithDescription("Returns the status of all low-level game hooks")
        .WithTags("Health")
        .Produces<HealthCheckResponse>(StatusCodes.Status200OK)
        .WithOpenApi();

        return app;
    }
}
