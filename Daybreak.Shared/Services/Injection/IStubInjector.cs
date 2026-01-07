using System.Diagnostics;

namespace Daybreak.Shared.Services.Injection;
public interface IStubInjector
{
    /// <summary>
    /// Injects the specified DLL into the target process using an asm stub. Only used for
    /// </summary>
    /// <returns></returns>
    Task<int> Inject(Process target, string dllPath, string entryPoint, CancellationToken cancellationToken);
}
