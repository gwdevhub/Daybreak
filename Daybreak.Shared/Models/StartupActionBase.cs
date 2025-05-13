using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Models;

public abstract class StartupActionBase
{
    public virtual void ExecuteOnStartup()
    {

    }

    public virtual Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
