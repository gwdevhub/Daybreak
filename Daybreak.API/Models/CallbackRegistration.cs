using System.Core.Extensions;

namespace Daybreak.API.Models;

public sealed class CallbackRegistration<T>(Guid uid, T callback, Action onDispose) : IDisposable
    where T : Delegate
{
    public Guid Uid { get; } = uid;
    public T Callback { get; } = callback.ThrowIfNull();
    private readonly Action onDispose = onDispose.ThrowIfNull();

    public void Dispose()
    {
        this.onDispose();
    }
}
