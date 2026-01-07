using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.Injection;

public interface IDaybreakInjector
{
    bool InjectorAvailable();
    Task<InjectorResponses.InjectResult> InjectWinApi(int processId, string dllPath, CancellationToken cancellationToken);
    Task<InjectorResponses.InjectResult> InjectStub(int processId, string dllPath, string entryPoint, CancellationToken cancellationToken);
    Task<(InjectorResponses.LaunchResult ExitCode, int ThreadHandle, int ProcessId)> Launch(string executablePath, bool elevated, string[] args, CancellationToken cancellationToken);
    Task<InjectorResponses.ResumeResult> Resume(int threadhandle, CancellationToken cancellationToken);
}
