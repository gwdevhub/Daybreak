using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.ExecutableManagement;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.ExecutableManagement;
internal sealed class GuildWarsExecutableManager(
    ILiveUpdateableOptions<GuildwarsExecutableOptions> liveUpdateableOptions,
    ILogger<GuildWarsExecutableManager> logger) : IGuildWarsExecutableManager, IApplicationLifetimeService
{
    private readonly static TimeSpan ExecutableVerificationLatency = TimeSpan.FromSeconds(5);
    private readonly static SemaphoreSlim ExecutablesSemaphore = new(1, 1);

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ILiveUpdateableOptions<GuildwarsExecutableOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<GuildWarsExecutableManager> logger = logger.ThrowIfNull();

    public IEnumerable<string> GetExecutableList()
    {
        ExecutablesSemaphore.Wait();

        var list = this.liveUpdateableOptions.Value.ExecutablePaths.Where(this.IsValidExecutable).ToList();
        ExecutablesSemaphore.Release();

        return list;
    }

    public void AddExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        ExecutablesSemaphore.Wait();

        var list = this.liveUpdateableOptions.Value.ExecutablePaths;
        if (list.None(e => e == executablePath))
        {
            list.Insert(0, executablePath);
        }

        this.liveUpdateableOptions.Value.ExecutablePaths = list;
        this.liveUpdateableOptions.UpdateOption();
        ExecutablesSemaphore.Release();
    }

    public void RemoveExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        ExecutablesSemaphore.Wait();

        var list = this.liveUpdateableOptions.Value.ExecutablePaths;
        if (list.Any(e => e == executablePath))
        {
            list.Remove(executablePath);
        }

        this.liveUpdateableOptions.Value.ExecutablePaths = list;
        this.liveUpdateableOptions.UpdateOption();
        ExecutablesSemaphore.Release();
    }

    public bool IsValidExecutable(string executablePath)
    {
        return IsValidExecutableInternal(executablePath);
    }

    public void OnClosing()
    {
    }

    public void OnStartup()
    {
        this.VerifyExecutables(this.cancellationTokenSource.Token);
    }

    private async void VerifyExecutables(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.VerifyExecutables), string.Empty);
        while (!cancellationToken.IsCancellationRequested)
        {
            await ExecutablesSemaphore.WaitAsync(cancellationToken);

            var executables = this.liveUpdateableOptions.Value.ExecutablePaths;
            var deletedExecutable = false;
            for(var i = 0; i < executables.Count; i++)
            {
                var executable = executables[i];
                if (IsValidExecutableInternal(executable))
                {
                    continue;
                }

                scopedLogger.LogWarning($"Detected deleted executable at {executable}");
                deletedExecutable = true;
                executables.Remove(executable);
                i--;
            }

            if (deletedExecutable)
            {
                this.liveUpdateableOptions.Value.ExecutablePaths = executables;
                this.liveUpdateableOptions.UpdateOption();
            }

            ExecutablesSemaphore.Release();
            await Task.Delay(ExecutableVerificationLatency, cancellationToken);
        }
    }

    private static bool IsValidExecutableInternal(string executablePath)
    {
        return File.Exists(executablePath);
    }
}
