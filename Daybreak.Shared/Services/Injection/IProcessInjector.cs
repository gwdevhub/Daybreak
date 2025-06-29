using System.Diagnostics;

namespace Daybreak.Shared.Services.Injection;
public interface IProcessInjector
{
    Task<bool> Inject(Process process, string pathToDll, CancellationToken cancellationToken);
}
