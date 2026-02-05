using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;
public sealed class UpdateToolboxAction(
    IToolboxService toolboxService,
    ILogger<UpdateToolboxAction> logger) : StartupActionBase
{
    private readonly IToolboxService toolboxService = toolboxService.ThrowIfNull();
    private readonly ILogger<UpdateToolboxAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (!this.toolboxService.IsInstalled)
        {
            scopedLogger.LogDebug("Toolbox is not installed. Skipping update check.");
            return;
        }

        scopedLogger.LogDebug("Checking toolbox for updates");
        Task.Factory.StartNew(async () => await this.toolboxService.NotifyUserIfUpdateAvailable(CancellationToken.None));
    }
}
