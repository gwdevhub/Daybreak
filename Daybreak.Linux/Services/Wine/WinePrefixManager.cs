using System.Diagnostics;
using Daybreak.Linux.Services.Startup.Notifications;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Wine;

/// <summary>
/// Manages a dedicated Wine prefix for running Windows executables on Linux.
/// The prefix is stored in the application root under WinePrefix/
/// Also implements IModService to appear in the mod manager UI for easy installation.
/// </summary>
public sealed class WinePrefixManager(
    INotificationService notificationService,
    ILogger<WinePrefixManager> logger) : IWinePrefixManager
{
    private const string WinePrefixFolder = "WinePrefix";
    private const string WineExecutable = "wine";

    private static readonly ProgressUpdate ProgressStarting = new(0, "Starting Wine prefix setup");
    private static readonly ProgressUpdate ProgressCheckingWine = new(
        0.1,
        "Checking Wine installation"
    );
    private static readonly ProgressUpdate ProgressCreatingDirectory = new(
        0.2,
        "Creating Wine prefix directory"
    );
    private static readonly ProgressUpdate ProgressInitializing = new(
        0.3,
        "Initializing Wine prefix (this may take a moment)"
    );
    private static readonly ProgressUpdate ProgressFinished = new(1, "Wine prefix setup complete");
    private static readonly ProgressUpdate ProgressAlreadyInitialized = new(
        1,
        "Wine prefix already initialized"
    );
    private static readonly ProgressUpdate ProgressFailed = new(1, "Failed to setup Wine prefix");

    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<WinePrefixManager> logger = logger;
    private readonly string winePrefixPath = PathUtils.GetAbsolutePathFromRoot(WinePrefixFolder);

    /// <inheritdoc />
    public string Name => "Wine Prefix";

    /// <inheritdoc />
    public string Description => "Wine prefix required to run Guild Wars and the injector on Linux. Wine must be installed on your system.";

    /// <inheritdoc />
    public bool IsEnabled
    {
        get => true;
        set { }
    }

    /// <inheritdoc />
    public bool IsInstalled => this.IsInitialized();

    /// <inheritdoc />
    public bool IsVisible => this.IsAvailable();

    /// <inheritdoc />
    public bool CanCustomManage => false;

    /// <inheritdoc />
    public bool CanUninstall => false;

    /// <inheritdoc />
    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        return this.Install(cancellationToken);
    }

    /// <inheritdoc />
    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(
            progress =>
            {
                progress.Report(new ProgressUpdate(0, "Removing Wine prefix"));

                if (!this.IsInstalled)
                {
                    progress.Report(new ProgressUpdate(1, "Wine prefix is not installed"));
                    return Task.FromResult(true);
                }

                try
                {
                    if (Directory.Exists(this.winePrefixPath))
                    {
                        Directory.Delete(this.winePrefixPath, recursive: true);
                    }

                    progress.Report(new ProgressUpdate(1, "Wine prefix removed successfully"));
                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to remove Wine prefix");
                    progress.Report(new ProgressUpdate(1, $"Failed to remove Wine prefix: {ex.Message}"));
                    return Task.FromResult(false);
                }
            },
            cancellationToken
        );
    }

    /// <inheritdoc />
    public IEnumerable<string> GetCustomArguments() => [];

    /// <inheritdoc />
    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        if (!this.IsAvailable())
        {
            this.logger.LogError("Wine is not installed. Cannot launch Guild Wars");
            guildWarsStartingContext.CancelStartup = true;
            this.notificationService.NotifyError(
                title: "Wine not installed",
                description: "Wine is required to launch Guild Wars on Linux. Please install Wine and restart Daybreak.",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        if (!this.IsInitialized())
        {
            this.logger.LogWarning("Wine prefix not initialized. Blocking Guild Wars startup");
            guildWarsStartingContext.CancelStartup = true;
            this.notificationService.NotifyError<WinePrefixSetupHandler>(
                title: "Wine prefix not initialized",
                description: "The Wine prefix needs to be set up before launching Guild Wars. Click here to initialize it.",
                expirationTime: DateTime.UtcNow + TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
        => Task.FromResult(false);

    /// <inheritdoc />
    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task OnCustomManagement(CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <inheritdoc />
    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken)
        => Task.FromResult(false);

    /// <inheritdoc />
    public Task<bool> PerformUpdate(CancellationToken cancellationToken)
        => Task.FromResult(true);

    public bool IsAvailable()
    {
        try
        {
            // Check if 'wine' command exists
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = WineExecutable,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
            };

            process.Start();
            process.WaitForExit();

            var isAvailable = process.ExitCode == 0;
            this.logger.LogInformation("Wine availability check: {IsAvailable}", isAvailable);
            return isAvailable;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to check Wine availability");
            return false;
        }
    }

    public bool IsInitialized()
    {
        // Check if the prefix directory exists and has been initialized
        // Wine creates a system.reg file when the prefix is initialized
        var systemRegPath = Path.Combine(this.winePrefixPath, "system.reg");
        return File.Exists(systemRegPath);
    }

    public string GetWinePrefixPath()
    {
        return this.winePrefixPath;
    }

    public IProgressAsyncOperation<bool> Install(CancellationToken cancellationToken)
    {
        return ProgressAsyncOperation.Create(
            async progress =>
                await Task
                    .Factory.StartNew(
                        () => this.InstallInternal(progress, cancellationToken),
                        cancellationToken,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Current
                    )
                    .Unwrap(),
            cancellationToken
        );
    }

    private async Task<bool> InstallInternal(
        IProgress<ProgressUpdate> progress,
        CancellationToken cancellationToken
    )
    {
        progress.Report(ProgressStarting);

        progress.Report(ProgressCheckingWine);
        if (!this.IsAvailable())
        {
            this.logger.LogError("Wine is not installed");
            progress.Report(ProgressFailed);
            return false;
        }

        if (this.IsInitialized())
        {
            this.logger.LogInformation(
                "Wine prefix already initialized at {PrefixPath}",
                this.winePrefixPath
            );
            progress.Report(ProgressAlreadyInitialized);
            return true;
        }

        this.logger.LogInformation("Initializing Wine prefix at {PrefixPath}", this.winePrefixPath);

        progress.Report(ProgressCreatingDirectory);
        Directory.CreateDirectory(this.winePrefixPath);

        progress.Report(ProgressInitializing);

        // Initialize the Wine prefix using wineboot
        var startInfo = new ProcessStartInfo
        {
            FileName = "wineboot",
            Arguments = "--init",
            WorkingDirectory = this.winePrefixPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        startInfo.Environment["WINEPREFIX"] = this.winePrefixPath;
        try
        {
            using var process = new Process { StartInfo = startInfo };
            process.Start();
            await process.WaitForExitAsync(cancellationToken);

            var output = await process.StandardOutput.ReadToEndAsync(cancellationToken);
            var error = await process.StandardError.ReadToEndAsync(cancellationToken);

            if (process.ExitCode != 0)
            {
                this.logger.LogError(
                    "Wine prefix initialization failed. Exit code: {ExitCode}, Error: {Error}",
                    process.ExitCode,
                    error
                );
                progress.Report(ProgressFailed);
                return false;
            }

            this.logger.LogInformation("Wine prefix initialized successfully");
            progress.Report(ProgressFinished);
            return true;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to initialize Wine prefix");
            progress.Report(ProgressFailed);
            return false;
        }
    }

    public async Task<(string? Output, string? Error, int ExitCode)> LaunchProcess(
        string exePath,
        string workingDirectory,
        string[] arguments,
        CancellationToken cancellationToken,
        Func<IReadOnlyList<string>, IReadOnlyList<string>, (bool IsComplete, int ExitCode)>? outputCompletionChecker = null,
        TimeSpan? timeout = null)
    {
        if (!this.IsAvailable())
        {
            this.logger.LogError("Wine is not available");
            return (null, "Wine is not installed", -1);
        }

        if (!this.IsInitialized())
        {
            this.logger.LogWarning("Wine prefix not initialized, initializing now...");
            var result = await this.Install(cancellationToken);
            if (!result)
            {
                return (null, "Failed to initialize Wine prefix", -1);
            }
        }

        var wineExePath = PathUtils.ToWinePath(exePath);
        var wineArgs = string.Join(' ', arguments);

        this.logger.LogDebug("Launching Wine process: {WineExePath} {Args}", wineExePath, wineArgs);

        var startInfo = new ProcessStartInfo
        {
            FileName = WineExecutable,
            Arguments = $"\"{wineExePath}\" {wineArgs}",
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        startInfo.Environment["WINEPREFIX"] = this.winePrefixPath;
        try
        {
            using var process = new Process { StartInfo = startInfo };

            var outputLines = new List<string>();
            var errorLines = new List<string>();
            var outputComplete = new TaskCompletionSource<bool>();
            var exitCodeFromOutput = 0;
            var foundExitCode = false;

            void CheckCompletion()
            {
                if (outputCompletionChecker is not null && !foundExitCode)
                {
                    var (isComplete, exitCode) = outputCompletionChecker(outputLines, errorLines);
                    if (isComplete)
                    {
                        foundExitCode = true;
                        exitCodeFromOutput = exitCode;
                        outputComplete.TrySetResult(true);
                    }
                }
            }

            process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data is not null)
                {
                    this.logger.LogDebug("Wine stdout: {Line}", e.Data);
                    outputLines.Add(e.Data);
                    CheckCompletion();
                }
                else
                {
                    // null data means end of stream
                    outputComplete.TrySetResult(true);
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data is not null)
                {
                    this.logger.LogDebug("Wine stderr: {Line}", e.Data);
                    errorLines.Add(e.Data);
                    CheckCompletion();
                }
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            var effectiveTimeout = timeout ?? TimeSpan.FromSeconds(30);
            using var timeoutCts = new CancellationTokenSource(effectiveTimeout);

            try
            {
                await Task.WhenAny(
                    outputComplete.Task,
                    process.WaitForExitAsync(timeoutCts.Token)
                );
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                this.logger.LogDebug("Wine process timeout reached after {Timeout}", effectiveTimeout);
            }

            try
            {
                await Task.Delay(50, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }

            var output = string.Join(Environment.NewLine, outputLines);
            var error = string.Join(Environment.NewLine, errorLines);

            int exitCode;
            if (process.HasExited)
            {
                exitCode = process.ExitCode;
            }
            else if (foundExitCode)
            {
                // We got the output we needed, but Wine hasn't exited (waiting for child)
                // Use the exit code we determined from the output
                exitCode = exitCodeFromOutput;
                this.logger.LogDebug("Wine process still running (child process alive), but we have the output we need");
            }
            else
            {
                // Timeout with no useful output
                exitCode = -999;
                this.logger.LogWarning("Wine process did not produce expected output within timeout");

                try
                {
                    process.Kill(entireProcessTree: true);
                }
                catch (Exception ex)
                {
                    this.logger.LogDebug(ex, "Failed to kill Wine process tree");
                }
            }

            this.logger.LogDebug(
                "Wine process result - ExitCode: {ExitCode}, Output: {Output}, Error: {Error}",
                exitCode,
                output,
                error
            );

            return (output, error, exitCode);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to launch Wine process");
            return (null, ex.Message, -1);
        }
    }
}
