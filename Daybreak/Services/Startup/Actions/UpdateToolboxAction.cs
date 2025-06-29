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

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.toolboxService.IsInstalled)
        {
            scopedLogger.LogDebug("Checking toolbox for updates");
            await this.toolboxService.NotifyUserIfUpdateAvailable(cancellationToken);
        }
    }
}
