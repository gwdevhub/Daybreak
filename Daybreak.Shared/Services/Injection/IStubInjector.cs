using System.Diagnostics;

namespace Daybreak.Shared.Services.Injection;
public interface IStubInjector
{
    Task<bool> Inject(Process target, string dllPath, CancellationToken cancellationToken);
}
