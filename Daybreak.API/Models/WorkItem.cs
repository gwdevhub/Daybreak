namespace Daybreak.API.Models;

public sealed class WorkItem(
    Action action,
    TaskCompletionSource tcs,
    CancellationToken token) : IWorkItem
{
    private readonly Action action = action;
    private readonly TaskCompletionSource tcs = tcs;

    public CancellationToken CancellationToken { get; } = token;

    public void Execute()
    {
        if (this.CancellationToken.IsCancellationRequested)
        {
            this.tcs.TrySetCanceled(this.CancellationToken);
            return;
        }

        try
        {
            this.action();
            this.tcs.TrySetResult();
        }
        catch (Exception ex)
        {
            this.tcs.TrySetException(ex);
        }
    }

    public void Cancel()
    {
        this.tcs.TrySetCanceled(this.CancellationToken);
    }

    public void Exception(Exception ex)
    {
        this.tcs.TrySetException(ex);
    }
}

public sealed class WorkItem<T>(
    Func<T> func,
    TaskCompletionSource<T> tcs,
    CancellationToken token) : IWorkItem
{
    private readonly Func<T> func = func;
    private readonly TaskCompletionSource<T> tcs = tcs;

    public CancellationToken CancellationToken { get; } = token;

    public void Execute()
    {
        if (this.CancellationToken.IsCancellationRequested)
        {
            this.tcs.TrySetCanceled(this.CancellationToken);
            return;
        }

        try
        {
            this.tcs.TrySetResult(this.func());
        }
        catch (Exception ex)
        {
            this.tcs.TrySetException(ex);
        }
    }

    public void Cancel()
    {
        this.tcs.TrySetCanceled(this.CancellationToken);
    }

    public void Exception(Exception ex)
    {
        this.tcs.TrySetException(ex);
    }
}
