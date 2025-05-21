using Daybreak.API.Serialization;
using Daybreak.API.Services.Interop;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Daybreak.API.Health;

public sealed class HooksHealthCheck(IEnumerable<IHookHealthService> hookHealthServices)
    : IHealthCheck
{
    private readonly IEnumerable<IHookHealthService> hookHealthServices = hookHealthServices;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var states = this.hookHealthServices.SelectMany(hookService => hookService.GetHookStates())
            .ToList();
        var healthy = !states.Any(s => !s.Hooked);
        var serialized = JsonSerializer.SerializeToElement(states, ApiJsonSerializerContext.Default.ListHookState);
        var meta = new Dictionary<string, object>
        {
            ["Count"] = states.Count.ToString(),
            ["Unhooked"] = states.Count(s => !s.Hooked).ToString(),
            ["States"] = serialized,
        };

        return Task.FromResult(healthy
            ? HealthCheckResult.Healthy("Hooks are initialized", meta)
            : HealthCheckResult.Unhealthy("Hooks are not initialized", data: meta));
    }
}
