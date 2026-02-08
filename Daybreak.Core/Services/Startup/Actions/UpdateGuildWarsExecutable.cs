using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Guildwars;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;

internal sealed class UpdateGuildWarsExecutable(
    IGuildWarsVersionChecker guildWarsVersionChecker,
    ILogger<UpdateGuildWarsExecutable> logger)
    : StartupActionBase
{
    private readonly IGuildWarsVersionChecker guildWarsVersionChecker = guildWarsVersionChecker.ThrowIfNull();
    private readonly ILogger<UpdateGuildWarsExecutable> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.guildWarsVersionChecker.IsEnabled)
        {
            scopedLogger.LogDebug("Guild Wars executable version checking is disabled. Skipping check.");
            return;
        }

        scopedLogger.LogInformation("Starting Guild Wars executable version check.");
        _ = new TaskFactory().StartNew(async () => await this.guildWarsVersionChecker.CheckExecutables(), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }
}
