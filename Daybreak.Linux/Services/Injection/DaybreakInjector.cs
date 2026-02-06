using System.Extensions.Core;
using Daybreak.Linux.Services.Wine;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Injection;

/// <summary>
/// Linux-specific implementation of IDaybreakInjector.
/// Uses Wine to run Daybreak.Injector.exe for launching and injecting into GuildWars.
/// </summary>
public class DaybreakInjector(
    ILogger<DaybreakInjector> logger,
    IWinePrefixManager winePrefixManager,
    IWinePidMapper winePidMapper
) : IDaybreakInjector
{
    private const string InjectorRelativePath = "Injector/Daybreak.Injector.exe";

    private readonly ILogger<DaybreakInjector> logger = logger;
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;
    private readonly IWinePidMapper winePidMapper = winePidMapper;

    public bool InjectorAvailable()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        // Check if Wine is installed and the injector executable exists
        if (!this.winePrefixManager.IsAvailable())
        {
            scopedLogger.LogWarning("Wine is not available");
            return false;
        }

        var injectorPath = PathUtils.GetAbsolutePathFromRoot(InjectorRelativePath);
        var exists = File.Exists(injectorPath);

        if (!exists)
        {
            scopedLogger.LogWarning("Injector not found at {InjectorPath}", injectorPath);
        }

        return exists;
    }

    public async Task<InjectorResponses.InjectResult> InjectWinApi(
        int processId,
        string dllPath,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available");
            return InjectorResponses.InjectResult.InvalidInjector;
        }

        // Convert Linux PID to Wine PID for the injector
        var winePid = this.winePidMapper.LinuxPidToWinePid(processId);
        if (winePid is null)
        {
            scopedLogger.LogError("No Wine PID mapping found for Linux PID {ProcessId}", processId);
            return InjectorResponses.InjectResult.InvalidProcess;
        }

        var wineDllPath = PathUtils.ToWinePath(dllPath);
        var (output, error, exitCode) = await this.LaunchInjector(
            ["winapi", winePid.Value.ToString(), $"\"{wineDllPath}\""],
            cancellationToken
        );

        scopedLogger.LogInformation(
            "Injector exit code: {ExitCode}\nInjector output: {Output}\nInjector error: {Error}",
            exitCode,
            output ?? string.Empty,
            error ?? string.Empty
        );

        return (InjectorResponses.InjectResult)exitCode;
    }

    public async Task<InjectorResponses.InjectResult> InjectStub(
        int processId,
        string dllPath,
        string entryPoint,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available");
            return InjectorResponses.InjectResult.InvalidInjector;
        }

        // Convert Linux PID to Wine PID for the injector
        var winePid = this.winePidMapper.LinuxPidToWinePid(processId);
        if (winePid is null)
        {
            scopedLogger.LogError("No Wine PID mapping found for Linux PID {ProcessId}", processId);
            return InjectorResponses.InjectResult.InvalidProcess;
        }

        var wineDllPath = PathUtils.ToWinePath(dllPath);
        var (output, error, exitCode) = await this.LaunchInjector(
            ["stub", winePid.Value.ToString(), $"\"{entryPoint}\"", $"\"{wineDllPath}\""],
            cancellationToken
        );

        scopedLogger.LogInformation(
            "Injector exit code: {ExitCode}\nInjector output: {Output}\nInjector error: {Error}",
            exitCode,
            output ?? string.Empty,
            error ?? string.Empty
        );

        return (InjectorResponses.InjectResult)exitCode;
    }

    public async Task<(
        InjectorResponses.LaunchResult ExitCode,
        int ThreadHandle,
        int ProcessId
    )> Launch(
        string executablePath,
        bool elevated,
        string[] args,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available");
            return (InjectorResponses.LaunchResult.InvalidInjector, -1, -1);
        }

        // Wine doesn't support SaferCreateLevel/SaferComputeTokenFromLevel APIs used for non-elevated launch.
        // Always use elevated mode on Linux to skip the restricted token creation.
        const bool forceElevated = true;
        if (!elevated)
        {
            scopedLogger.LogDebug("Forcing elevated mode for Wine compatibility");
        }

        var wineExePath = PathUtils.ToWinePath(executablePath);

        // Completion checker: We're done when we see both ProcessId and ThreadHandle
        static (bool IsComplete, int ExitCode) LaunchCompletionChecker(
            IReadOnlyList<string> outputLines,
            IReadOnlyList<string> errorLines)
        {
            var hasProcessId = outputLines.Any(l => l.StartsWith("ProcessId: "));
            var hasThreadHandle = outputLines.Any(l => l.StartsWith("ThreadHandle: "));

            if (hasProcessId && hasThreadHandle)
            {
                return (true, 0); // Success
            }

            return (false, 0);
        }

        var (output, error, exitCode) = await this.LaunchInjector(
            ["launch", forceElevated.ToString(), $"\"{wineExePath}\"", string.Join(' ', args)],
            cancellationToken,
            LaunchCompletionChecker,
            TimeSpan.FromSeconds(15) // Short timeout - we just need ProcessId/ThreadHandle output
        );

        scopedLogger.LogInformation(
            "Injector exit code: {ExitCode}\nInjector output: {Output}\nInjector error: {Error}",
            exitCode,
            output ?? string.Empty,
            error ?? string.Empty
        );

        // Parse the output for process ID and thread handle
        var lines =
            output?.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries) ?? [];
        var processId = 0;
        var threadHandle = 0;

        foreach (var line in lines)
        {
            if (line.StartsWith("ProcessId: "))
            {
                var processIdString = line["ProcessId: ".Length..];
                _ = int.TryParse(processIdString, out processId);
            }
            else if (line.StartsWith("ThreadHandle: "))
            {
                var threadHandleString = line["ThreadHandle: ".Length..];
                _ = int.TryParse(threadHandleString, out threadHandle);
            }
        }

        // Convert Wine PID to Linux PID so the rest of Daybreak can use Process.GetProcessById()
        if (processId > 0)
        {
            var executableName = Path.GetFileName(executablePath);
            var linuxPid = this.winePidMapper.WinePidToLinuxPid(processId, executableName);
            if (linuxPid is not null)
            {
                scopedLogger.LogInformation(
                    "Mapped Wine PID {WinePid} to Linux PID {LinuxPid}",
                    processId,
                    linuxPid.Value
                );
                processId = linuxPid.Value;
            }
            else
            {
                scopedLogger.LogWarning(
                    "Could not map Wine PID {WinePid} to Linux PID. Process.GetProcessById will likely fail",
                    processId
                );
            }
        }

        return ((InjectorResponses.LaunchResult)exitCode, threadHandle, processId);
    }

    public async Task<InjectorResponses.ResumeResult> Resume(
        int threadHandle,
        CancellationToken cancellationToken
    )
    {
        var scopedLogger = this.logger.CreateScopedLogger();

        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available");
            return InjectorResponses.ResumeResult.InvalidInjector;
        }

        var (output, error, exitCode) = await this.LaunchInjector(
            ["resume", threadHandle.ToString()],
            cancellationToken
        );

        scopedLogger.LogInformation(
            "Injector exit code: {ExitCode}\nInjector output: {Output}\nInjector error: {Error}",
            exitCode,
            output ?? string.Empty,
            error ?? string.Empty
        );

        return (InjectorResponses.ResumeResult)exitCode;
    }

    private async Task<(string? Output, string? Error, int ExitCode)> LaunchInjector(
        string[] arguments,
        CancellationToken cancellationToken,
        Func<IReadOnlyList<string>, IReadOnlyList<string>, (bool IsComplete, int ExitCode)>? outputCompletionChecker = null,
        TimeSpan? timeout = null
    )
    {
        var injectorPath = PathUtils.GetAbsolutePathFromRoot(InjectorRelativePath);
        var workingDirectory = Path.GetDirectoryName(injectorPath) ?? PathUtils.GetRootFolder();

        return await this.winePrefixManager.LaunchProcess(
            injectorPath,
            workingDirectory,
            arguments,
            cancellationToken,
            outputCompletionChecker,
            timeout
        );
    }
}
