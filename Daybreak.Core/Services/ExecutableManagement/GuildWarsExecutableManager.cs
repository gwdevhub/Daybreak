using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;

namespace Daybreak.Services.ExecutableManagement;

internal sealed class GuildWarsExecutableManager(
    IOptionsProvider optionsProvider,
    IOptionsMonitor<GuildwarsExecutableOptions> liveUpdateableOptions,
    ILogger<GuildWarsExecutableManager> logger) : IGuildWarsExecutableManager, IHostedService
{
    private readonly static TimeSpan ExecutableVerificationLatency = TimeSpan.FromSeconds(5);
    private readonly static SemaphoreSlim ExecutablesSemaphore = new(1, 1);

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<GuildwarsExecutableOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<GuildWarsExecutableManager> logger = logger.ThrowIfNull();

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        await this.VerifyExecutables(cancellationToken);
    }

    Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public IEnumerable<string> GetExecutableList()
    {
        ExecutablesSemaphore.Wait();

        var list = this.liveUpdateableOptions.CurrentValue.ExecutablePaths.Where(this.IsValidExecutable).ToList();
        ExecutablesSemaphore.Release();

        return list;
    }

    public void AddExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        ExecutablesSemaphore.Wait();

        var options = this.liveUpdateableOptions.CurrentValue;
        var list = options.ExecutablePaths;
        if (list.None(e => e == executablePath))
        {
            list.Insert(0, executablePath);
        }

        options.ExecutablePaths = list;
        this.optionsProvider.SaveOption(options);
        ExecutablesSemaphore.Release();
    }

    public void RemoveExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        ExecutablesSemaphore.Wait();

        var options = this.liveUpdateableOptions.CurrentValue;
        var list = options.ExecutablePaths;
        if (list.Any(e => e == executablePath))
        {
            list.Remove(executablePath);
        }

        options.ExecutablePaths = list;
        this.optionsProvider.SaveOption(options);
        ExecutablesSemaphore.Release();
    }

    public bool IsValidExecutable(string executablePath)
    {
        return IsValidExecutableInternal(executablePath);
    }

    private async Task VerifyExecutables(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.VerifyExecutables), string.Empty);
        while (!cancellationToken.IsCancellationRequested)
        {
            await ExecutablesSemaphore.WaitAsync(cancellationToken);

            var executables = this.liveUpdateableOptions.CurrentValue.ExecutablePaths;
            var deletedExecutable = false;
            for (var i = 0; i < executables.Count; i++)
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
                var options = this.liveUpdateableOptions.CurrentValue;
                options.ExecutablePaths = executables;
                this.optionsProvider.SaveOption(options);
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
