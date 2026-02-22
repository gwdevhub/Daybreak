using Daybreak.API.Models;

namespace Daybreak.API.Services.Interop;

public sealed class ChatHandlingService
    : IHostedService, IHookHealthService
{
    private CancellationTokenSource? cts = default;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Cancel();
        this.cts?.Dispose();
        return Task.CompletedTask;
    }

    public List<HookState> GetHookStates()
    {
        return
            [
            ];
    }
}
