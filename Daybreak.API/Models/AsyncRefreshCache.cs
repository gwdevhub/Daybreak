using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.API.Models;

public sealed class AsyncRefreshCache<T>(TimeSpan refreshRate, Func<CancellationToken, ValueTask<T>> refreshFunc)
{
    private readonly Func<CancellationToken, ValueTask<T>> refreshFunc = refreshFunc.ThrowIfNull();
    
    private readonly SemaphoreSlim semaphoreSlim = new(1);
    
    private T? cache;
    private DateTime lastUpdateDateTime = DateTime.MinValue;
    private TimeSpan refreshRate = refreshRate;

    public async ValueTask<T> GetAsync(CancellationToken cancellationToken)
    {
        if (this.cache is not null && DateTime.UtcNow - this.lastUpdateDateTime < this.refreshRate)
        {
            return this.cache;
        }

        using var ctx = await this.semaphoreSlim.Acquire(cancellationToken);
        if (DateTime.UtcNow - this.lastUpdateDateTime > this.refreshRate ||
            this.cache is null)
        {
            this.lastUpdateDateTime = DateTime.UtcNow;
            this.cache = await this.refreshFunc(cancellationToken);
        }

        return this.cache;
    }

    public void UpdateRefreshRate(TimeSpan newRefreshRate)
    {
        this.refreshRate = newRefreshRate;
    }
}
