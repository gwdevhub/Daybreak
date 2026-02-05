using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Keyboard;
using Microsoft.Extensions.Hosting;

namespace Daybreak.Linux.Services.Keyboard;

// TODO: Implement proper Linux keyboard hook using X11/Wayland APIs or libinput
public sealed class KeyboardHookService : IHostedService, IKeyboardHookService, IDisposable
{
    public event EventHandler<KeyboardEventArgs>? KeyDown;
    public event EventHandler<KeyboardEventArgs>? KeyUp;

    Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Start()
    {
        // TODO: Implement Linux keyboard hook
    }

    public void Stop()
    {
        // TODO: Implement Linux keyboard hook cleanup
    }

    public void Dispose()
    {
        this.Stop();
    }
}
