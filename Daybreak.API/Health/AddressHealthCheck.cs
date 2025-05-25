using Daybreak.API.Serialization;
using Daybreak.API.Services.Interop;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using ZLinq;

namespace Daybreak.API.Health;

public sealed class AddressHealthCheck(IEnumerable<IAddressHealthService> addressHealthServices)
    : IHealthCheck
{
    private readonly IEnumerable<IAddressHealthService> addressHealthServices = addressHealthServices;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var states = this.addressHealthServices.AsValueEnumerable().SelectMany(addressService => addressService.GetAddressStates());
        var healthy = !states.Any(s => s.Address == 0x0);
        using var statesArray = states.ToArrayPool();
        var serialized = JsonSerializer.SerializeToElement(statesArray.ArraySegment, ApiJsonSerializerContext.Default.ArraySegmentAddressState);
        var meta = new Dictionary<string, object>
        {
            ["Count"] = statesArray.Size,
            ["Invalid"] = states.Count(s => s.Address == 0x0),
            ["States"] = serialized,
        };

        return Task.FromResult(healthy
            ? HealthCheckResult.Healthy("Addresses are initialized", meta)
            : HealthCheckResult.Unhealthy("Addresses are not initialized", data: meta));
    }
}
