using Daybreak.Configuration.Options;
using Daybreak.Shared.Services.ExecutableManagement;
using Daybreak.Shared.Services.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.ExecutableManagement;

internal sealed class GuildWarsExecutableManager(
    IOptionsProvider optionsProvider,
    IOptionsMonitor<GuildwarsExecutableOptions> liveUpdateableOptions,
    ILogger<GuildWarsExecutableManager> logger) : IGuildWarsExecutableManager
{
    internal const int MaxExecutables = 10;
    private readonly static SemaphoreSlim ExecutablesSemaphore = new(1, 1);

    private readonly IOptionsProvider optionsProvider = optionsProvider.ThrowIfNull();
    private readonly IOptionsMonitor<GuildwarsExecutableOptions> liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull();
    private readonly ILogger<GuildWarsExecutableManager> logger = logger.ThrowIfNull();

    public IEnumerable<string> GetExecutableList()
    {
        ExecutablesSemaphore.Wait();

        var list = this.liveUpdateableOptions.CurrentValue.ExecutablePaths.Where(this.IsValidExecutable).ToList();
        ExecutablesSemaphore.Release();

        return list;
    }

    public void AddExecutable(string executablePath)
    {
        ExecutablesSemaphore.Wait();

        var options = this.liveUpdateableOptions.CurrentValue;
        var list = options.ExecutablePaths;
        if (list.None(e => e == executablePath))
        {
            list.Insert(0, executablePath);
        }

        this.TrimInvalidExecutables(list);

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

    /// <summary>
    /// Soft-caps the stored executable list at <see cref="MaxExecutables"/>. Only entries that no
    /// longer point to an existing file are evicted, oldest first, and only while the list is over
    /// capacity. A valid executable is never evicted automatically, even if that keeps the list
    /// above the cap. This bounds growth from stale entries without losing executables that live on
    /// a temporarily unavailable volume (e.g. an unmounted removable or network drive), which would
    /// otherwise be indistinguishable from a deleted file by path alone.
    /// </summary>
    private void TrimInvalidExecutables(List<string> executables)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        for (var i = executables.Count - 1; i >= 0 && executables.Count > MaxExecutables; i--)
        {
            if (IsValidExecutableInternal(executables[i]))
            {
                continue;
            }

            scopedLogger.LogInformation("Evicting stale executable while over capacity: {executable}", executables[i]);
            executables.RemoveAt(i);
        }
    }

    private static bool IsValidExecutableInternal(string executablePath)
    {
        return File.Exists(executablePath);
    }
}
