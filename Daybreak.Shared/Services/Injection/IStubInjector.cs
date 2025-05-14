using System.Diagnostics;

namespace Daybreak.Shared.Services.Injection;
public interface IStubInjector
{
    bool Inject(Process target, string dllPath, out int exitCode);
}
