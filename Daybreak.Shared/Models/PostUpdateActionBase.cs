using System.Threading.Tasks;

namespace Daybreak.Models;

public abstract class PostUpdateActionBase
{
    public virtual void DoPostUpdateAction()
    {
    }

    public virtual Task DoPostUpdateActionAsync()
    {
        return Task.CompletedTask;
    }
}
