using Daybreak.API.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Core.Extensions;

namespace Daybreak.API.Health;

public sealed class GameThreadHookCheck(GameThreadService gameThreadService)
    : IHealthCheck
{
    private readonly GameThreadService gameThreadService = gameThreadService.ThrowIfNull();

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var state = this.gameThreadService.GetHookState();
        var meta = new Dictionary<string, object>()
        {
            { "Hooked", state.Hooked },
            { "DetourAddress", state.DetourAddress },
            { "ContinueAddress", state.ContinueAddress },
            { "TargetAddress", state.TargetAddress }
        };
        return Task.FromResult(state.Hooked
            ? HealthCheckResult.Healthy("Game thread hook is initialized", meta)
            : HealthCheckResult.Unhealthy("Game thread hook is not initialized", data: meta));
    }
}
