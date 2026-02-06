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
    IWinePrefixManager winePrefixManager
) : IDaybreakInjector
{
    private const string InjectorRelativePath = "Injector/Daybreak.Injector.exe";

    private readonly ILogger<DaybreakInjector> logger = logger;
    private readonly IWinePrefixManager winePrefixManager = winePrefixManager;

    public bool InjectorAvailable()
    {
        // Check if Wine is installed and the injector executable exists
        if (!this.winePrefixManager.IsAvailable())
        {
            this.logger.LogWarning("Wine is not available");
            return false;
        }

        var injectorPath = PathUtils.GetAbsolutePathFromRoot(InjectorRelativePath);
        var exists = File.Exists(injectorPath);

        if (!exists)
        {
            this.logger.LogWarning("Injector not found at {InjectorPath}", injectorPath);
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

        var wineDllPath = PathUtils.ToWinePath(dllPath);
        var (output, error, exitCode) = await this.LaunchInjector(
            ["winapi", processId.ToString(), $"\"{wineDllPath}\""],
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

        var wineDllPath = PathUtils.ToWinePath(dllPath);
        var (output, error, exitCode) = await this.LaunchInjector(
            ["stub", processId.ToString(), $"\"{entryPoint}\"", $"\"{wineDllPath}\""],
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

        var wineExePath = PathUtils.ToWinePath(executablePath);
        var (output, error, exitCode) = await this.LaunchInjector(
            ["launch", elevated.ToString(), $"\"{wineExePath}\"", string.Join(' ', args)],
            cancellationToken
        );

        scopedLogger.LogInformation(
            "Injector exit code: {ExitCode}\nInjector output: {Output}\nInjector error: {Error}",
            exitCode,
            output ?? string.Empty,
            error ?? string.Empty
        );

        // Parse the output for process ID and thread handle
        var lines =
            output?.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries) ?? [];
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
        CancellationToken cancellationToken
    )
    {
        var injectorPath = PathUtils.GetAbsolutePathFromRoot(InjectorRelativePath);
        var workingDirectory = Path.GetDirectoryName(injectorPath) ?? PathUtils.GetRootFolder();

        return await this.winePrefixManager.LaunchProcess(
            injectorPath,
            workingDirectory,
            arguments,
            cancellationToken
        );
    }
}
