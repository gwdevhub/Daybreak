using System.Diagnostics;

namespace Daybreak.Services.Injection;
public interface IProcessInjector
{
    bool Inject(Process process, string pathToDll);
}
