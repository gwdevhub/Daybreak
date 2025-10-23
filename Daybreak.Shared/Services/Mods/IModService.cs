using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;

namespace Daybreak.Shared.Services.Mods;

public interface IModService
{
    /// <summary>
    /// The name of the mod. Displayed in the mod manager UI.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The description of the mod. Displayed in the mod manager UI.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the mod is enabled. If true, the mod will be applied when starting Guild Wars.
    /// </summary>
    bool IsEnabled { get; set; }

    /// <summary>
    /// True if the mod is intalled in the Daybreak staging directory and ready to be used.
    /// </summary>
    bool IsInstalled { get; }

    /// <summary>
    /// Dictates if the mod is visible in the mod manager UI.
    /// </summary>
    bool IsVisible { get; }

    /// <summary>
    /// Dictates if the mod has custom management.
    /// </summary>
    bool CanCustomManage { get; }

    /// <summary>
    /// Called by the mod manager to perform installation steps for the mod.
    /// </summary>
    /// <returns>Returns an awaitable <see cref="IProgressAsyncOperation{bool}"/></returns>
    /// <remarks>
    /// See <see cref="ProgressAsyncOperation{T}"/> on how to get progress reports and await the operation.
    /// </remarks>
    IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken);

    /// <summary>
    /// Custom command-line arguments to pass to the Guild Wars process.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Called when the user requests custom management of the mod.
    /// </summary>
    /// <remarks>
    /// This method is only called if <see cref="CanCustomManage"/> is true.
    /// </remarks>
    Task OnCustomManagement(CancellationToken cancellationToken);
}
