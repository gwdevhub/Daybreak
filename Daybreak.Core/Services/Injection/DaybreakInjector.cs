using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Injection;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Extensions.Core;

namespace Daybreak.Services.Injection;

public class DaybreakInjector(
    ILogger<DaybreakInjector> logger)
    : IDaybreakInjector
{
    private const string ExecutableName = "Injector/Daybreak.Injector.exe";

    private readonly ILogger<DaybreakInjector> logger = logger;

    public bool InjectorAvailable()
    {
        var executablePath = PathUtils.GetAbsolutePathFromRoot(ExecutableName);
        return File.Exists(executablePath);
    }

    public async Task<InjectorResponses.InjectResult> InjectWinApi(int processId, string dllPath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available at path {InjectorPath}", PathUtils.GetAbsolutePathFromRoot(ExecutableName));
            return InjectorResponses.InjectResult.InvalidInjector;
        }

        var (output, error, exitCode) = await LaunchInjector(
            [
                "winapi",
                processId.ToString(),
                $"\"{dllPath}\""
            ],
            cancellationToken);

        scopedLogger.LogInformation("Injector exit code: {exitCode}\nInjector output: {output}\nInjector error: {error}", exitCode, output ?? string.Empty, error ?? string.Empty);
        return (InjectorResponses.InjectResult)exitCode;
    }

    public async Task<InjectorResponses.InjectResult> InjectStub(int processId, string dllPath, string entryPoint, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available at path {InjectorPath}", PathUtils.GetAbsolutePathFromRoot(ExecutableName));
            return InjectorResponses.InjectResult.InvalidInjector;
        }

        var (output, error, exitCode) = await LaunchInjector(
            [
                "stub",
                processId.ToString(),
                $"\"{entryPoint}\"",
                $"\"{dllPath}\""
            ],
            cancellationToken);

        scopedLogger.LogInformation("Injector exit code: {exitCode}\nInjector output: {output}\nInjector error: {error}", exitCode, output ?? string.Empty, error ?? string.Empty);
        return (InjectorResponses.InjectResult)exitCode;
    }

    public async Task<(InjectorResponses.LaunchResult ExitCode, int ThreadHandle, int ProcessId)> Launch(string executablePath, bool elevated, string[] args, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available at path {InjectorPath}", PathUtils.GetAbsolutePathFromRoot(ExecutableName));
            return (InjectorResponses.LaunchResult.InvalidInjector, -1, -1);
        }

        var (output, error, exitCode) = await LaunchInjector(
            [
                "launch",
                elevated.ToString(),
                $"\"{executablePath}\"",
                string.Join(' ', args)
            ],
            cancellationToken);

        scopedLogger.LogInformation("Injector exit code: {exitCode}\nInjector output: {output}\nInjector error: {error}", exitCode, output ?? string.Empty, error ?? string.Empty);
        var lines = output?.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries) ?? [];
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

    public async Task<InjectorResponses.ResumeResult> Resume(int threadhandle, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.InjectorAvailable())
        {
            scopedLogger.LogError("Injector not available at path {InjectorPath}", PathUtils.GetAbsolutePathFromRoot(ExecutableName));
            return InjectorResponses.ResumeResult.InvalidInjector;
        }

        var (output, error, exitCode) = await LaunchInjector(
            [
                "resume",
                threadhandle.ToString()
            ],
            cancellationToken);

        scopedLogger.LogInformation("Injector exit code: {exitCode}\nInjector output: {output}\nInjector error: {error}", exitCode, output ?? string.Empty, error ?? string.Empty);
        return (InjectorResponses.ResumeResult)exitCode;
    }

    private static async Task<(string? Output, string? Error, int ExitCode)> LaunchInjector(string[] arguments, CancellationToken cancellationToken)
    {
        var executablePath = PathUtils.GetAbsolutePathFromRoot(ExecutableName);
        if (!File.Exists(executablePath))
        {
            throw new InvalidOperationException($"Could not find injector {executablePath}");
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = executablePath,
                Arguments = string.Join(' ', arguments),
                WorkingDirectory = Path.GetDirectoryName(executablePath),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = new Process
            {
                StartInfo = startInfo,
            };

            process.Start();
            await process.WaitForExitAsync(cancellationToken);

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = await process.StandardError.ReadToEndAsync(cancellationToken);
            return (output, error, process.ExitCode);
        }
        catch(Exception)
        {
            throw;
        }
    }
}
