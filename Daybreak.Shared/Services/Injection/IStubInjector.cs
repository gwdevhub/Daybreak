using System.Diagnostics;

namespace Daybreak.Services.Injection;
public interface IStubInjector
{
    bool Inject(Process target, string dllPath);
}
