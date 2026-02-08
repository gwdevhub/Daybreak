using Daybreak.Shared.Models;
using Daybreak.Shared.Services.ReShade;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;

internal sealed class UpdateReShadeAction(
    IReShadeService reShadeService,
    ILogger<UpdateReShadeAction> logger)
    : StartupActionBase
{
    private readonly IReShadeService reShadeService = reShadeService.ThrowIfNull();
    private readonly ILogger<UpdateReShadeAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.reShadeService.IsInstalled)
        {
            scopedLogger.LogDebug("ReShade update is not installed. Skipping check.");
            return;
        }

        scopedLogger.LogInformation("Starting ReShade update check.");
        Task.Factory.StartNew(async () => await this.reShadeService.CheckUpdates(CancellationToken.None));
    }
}
