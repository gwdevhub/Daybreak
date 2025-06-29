using Daybreak.Shared.Models.Mods;

namespace Daybreak.Shared.Services.Mods;

public interface IModService
{
    string Name { get; }
    bool IsEnabled { get; set; }
    bool IsInstalled { get; }
    IEnumerable<string> GetCustomArguments();
    /// <summary>
    /// Called before starting the guild wars process.
    /// Do mod preparation here.
    /// </summary>
    Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken);
    /// <summary>
    /// Called when the process is created in suspended state.
    /// Do dll injection here.
    /// </summary>
    Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken);
    /// <summary>
    /// Called after the process has been resumed.
    /// Do clean-up/integration with the guild wars process here.
    /// </summary>
    Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken);
    /// <summary>
    /// Called before starting the guild wars process, when the mod is disabled.
    /// Use this method to clean up the GuildWars folder of any residual mod files.
    /// </summary>
    Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken);

    /// <summary>
    /// Called periodically while the guild wars process is running.
    /// Use this method to detect if the mod should be re-run. (eg. if the running Guild Wars instance is missing modules)
    /// </summary>
    /// <returns>True if this mod should run again on the guild wars process</returns>
    Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken);

    /// <summary>
    /// Called if <see cref="ShouldRunAgain(GuildWarsRunningContext, CancellationToken)"/> returned true.
    /// Use this method to re-inject or re-apply the mod to the running Guild Wars process.
    /// </summary>
    Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken);
}
