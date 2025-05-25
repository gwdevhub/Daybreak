using System.Core.Extensions;

namespace Daybreak.API.Models;

public sealed class CallbackRegistration(Guid uid, Action onDispose) : IDisposable
{
    private readonly Guid uid = uid;
    private readonly Action onDispose = onDispose.ThrowIfNull();

    public void Dispose()
    {
        this.onDispose();
    }
}
