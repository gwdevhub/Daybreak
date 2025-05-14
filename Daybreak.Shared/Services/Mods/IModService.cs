using Daybreak.Shared.Models.Mods;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
}
