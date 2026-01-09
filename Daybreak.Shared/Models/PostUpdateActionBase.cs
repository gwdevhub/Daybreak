namespace Daybreak.Shared.Models;

public abstract class PostUpdateActionBase
{
    public virtual void DoPostUpdateAction()
    {
    }

    public virtual Task DoPostUpdateActionAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
