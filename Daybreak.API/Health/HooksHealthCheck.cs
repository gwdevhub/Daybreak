using Daybreak.API.Serialization;
using Daybreak.API.Services.Interop;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using ZLinq;

namespace Daybreak.API.Health;

public sealed class HooksHealthCheck(IEnumerable<IHookHealthService> hookHealthServices)
    : IHealthCheck
{
    private readonly IEnumerable<IHookHealthService> hookHealthServices = hookHealthServices;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var states = this.hookHealthServices.AsValueEnumerable().SelectMany(hookService => hookService.GetHookStates());
        var healthy = !states.Any(s => !s.Hooked);
        var statesArray = states.ToArrayPool();
        var serialized = JsonSerializer.SerializeToElement(statesArray.ArraySegment, ApiJsonSerializerContext.Default.ArraySegmentHookState);
        var meta = new Dictionary<string, object>
        {
            ["Count"] = statesArray.Size,
            ["Unhooked"] = states.Count(s => !s.Hooked),
            ["States"] = serialized,
        };

        return Task.FromResult(healthy
            ? HealthCheckResult.Healthy("Hooks are initialized", meta)
            : HealthCheckResult.Unhealthy("Hooks are not initialized", data: meta));
    }
}
