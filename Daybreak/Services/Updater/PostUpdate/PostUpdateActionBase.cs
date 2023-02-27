using System.Threading.Tasks;

namespace Daybreak.Services.Updater.PostUpdate;

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
