using Daybreak.API.Serialization;
using Daybreak.API.Services.Interop;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace Daybreak.API.Health;

public sealed class AddressHealthCheck(IEnumerable<IAddressHealthService> addressHealthServices)
    : IHealthCheck
{
    private readonly IEnumerable<IAddressHealthService> addressHealthServices = addressHealthServices;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var states = this.addressHealthServices.SelectMany(addressService => addressService.GetAddressStates())
            .ToList();
        var healthy = !states.Any(s => s.Address == 0x0);
        var serialized = JsonSerializer.SerializeToElement(states, ApiJsonSerializerContext.Default.ListAddressState);
        var meta = new Dictionary<string, object>
        {
            ["Count"] = states.Count.ToString(),
            ["Invalid"] = states.Count(s => s.Address == 0x0).ToString(),
            ["States"] = serialized,
        };

        return Task.FromResult(healthy
            ? HealthCheckResult.Healthy("Addresses are initialized", meta)
            : HealthCheckResult.Unhealthy("Addresses are not initialized", data: meta));
    }
}
