using Daybreak.Configuration.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Extensions.Services;

namespace Daybreak.Services.ExecutableManagement;
internal sealed class GuildWarsExecutableManager : IGuildWarsExecutableManager, IApplicationLifetimeService
{
    private readonly static TimeSpan ExecutableVerificationLatency = TimeSpan.FromSeconds(5);
    private readonly static object Lock = new();

    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ILiveUpdateableOptions<GuildwarsExecutableOptions> liveUpdateableOptions;
    private readonly ILogger<GuildWarsExecutableManager> logger;

    public GuildWarsExecutableManager(
        ILiveUpdateableOptions<GuildwarsExecutableOptions> liveUpdateableOptions,
        ILogger<GuildWarsExecutableManager> logger)
    {
        this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public IEnumerable<string> GetExecutableList()
    {
        while (!Monitor.TryEnter(Lock)) { }

        var list = this.liveUpdateableOptions.Value.ExecutablePaths.Where(this.IsValidExecutable).ToList();
        Monitor.Exit(Lock);

        return list;
    }

    public void AddExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        while (!Monitor.TryEnter(Lock)) { }

        var list = this.liveUpdateableOptions.Value.ExecutablePaths;
        if (list.None(e => e == executablePath))
        {
            list.Add(executablePath);
        }

        this.liveUpdateableOptions.Value.ExecutablePaths = list;
        this.liveUpdateableOptions.UpdateOption();
        Monitor.Exit(Lock);
    }

    public void RemoveExecutable(string executablePath)
    {
        var fullPath = Path.GetFullPath(executablePath);
        while (!Monitor.TryEnter(Lock)) { }

        var list = this.liveUpdateableOptions.Value.ExecutablePaths;
        if (list.Any(e => e == executablePath))
        {
            list.Remove(executablePath);
        }

        this.liveUpdateableOptions.Value.ExecutablePaths = list;
        this.liveUpdateableOptions.UpdateOption();
        Monitor.Exit(Lock);
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
            while (!Monitor.TryEnter(Lock)) { }

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

            Monitor.Exit(Lock);
            await Task.Delay(ExecutableVerificationLatency, cancellationToken);
        }
    }

    private static bool IsValidExecutableInternal(string executablePath)
    {
        return File.Exists(executablePath);
    }
}
