using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Injection;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Injection;

// TODO: Implement Wine-based injection for Linux
// This implementation needs to:
// 1. Set up Wine prefix if not exists (WINEPREFIX=/path wineboot --init)
// 2. Call "wine Daybreak.Injector.exe <args>" with WINEPREFIX environment variable
// 3. Parse output the same way as Windows version
// 4. Handle Wine-specific process ID mapping (Wine PIDs vs Linux PIDs)
/// <summary>
/// Linux-specific implementation of IDaybreakInjector.
/// Uses Wine to run Daybreak.Injector.exe for launching and injecting into GuildWars.
/// </summary>
public class DaybreakInjector(
    ILogger<DaybreakInjector> logger)
    : IDaybreakInjector
{
    private readonly ILogger<DaybreakInjector> logger = logger;

    public bool InjectorAvailable()
    {
        // TODO: Check if Wine is installed and Daybreak.Injector.exe exists
        this.logger.LogWarning("Linux DaybreakInjector.InjectorAvailable not implemented");
        return false;
    }

    public Task<InjectorResponses.InjectResult> InjectWinApi(int processId, string dllPath, CancellationToken cancellationToken)
    {
        // TODO: Implement via Wine
        this.logger.LogWarning("Linux DaybreakInjector.InjectWinApi not implemented");
        return Task.FromResult(InjectorResponses.InjectResult.InvalidInjector);
    }

    public Task<InjectorResponses.InjectResult> InjectStub(int processId, string dllPath, string entryPoint, CancellationToken cancellationToken)
    {
        // TODO: Implement via Wine
        this.logger.LogWarning("Linux DaybreakInjector.InjectStub not implemented");
        return Task.FromResult(InjectorResponses.InjectResult.InvalidInjector);
    }

    public Task<(InjectorResponses.LaunchResult ExitCode, int ThreadHandle, int ProcessId)> Launch(string executablePath, bool elevated, string[] args, CancellationToken cancellationToken)
    {
        // TODO: Implement via Wine
        // wine Daybreak.Injector.exe launch <elevated> "<executablePath>" <args>
        this.logger.LogWarning("Linux DaybreakInjector.Launch not implemented");
        return Task.FromResult((InjectorResponses.LaunchResult.InvalidInjector, -1, -1));
    }

    public Task<InjectorResponses.ResumeResult> Resume(int threadhandle, CancellationToken cancellationToken)
    {
        // TODO: Implement via Wine
        this.logger.LogWarning("Linux DaybreakInjector.Resume not implemented");
        return Task.FromResult(InjectorResponses.ResumeResult.InvalidInjector);
    }
}
