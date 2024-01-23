using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Injection;
public interface IProcessInjector
{
    Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken);
}
