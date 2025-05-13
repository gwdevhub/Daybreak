using Daybreak.Models;
using Daybreak.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Startup.Actions;
public sealed class UpdateToolboxAction : StartupActionBase
{
    private readonly IToolboxService toolboxService;
    private readonly ILogger<UpdateToolboxAction> logger;

    public UpdateToolboxAction(
        IToolboxService toolboxService,
        ILogger<UpdateToolboxAction> logger)
    {
        this.toolboxService = toolboxService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.toolboxService.IsInstalled)
        {
            scopedLogger.LogInformation("Checking toolbox for updates");
            await this.toolboxService.NotifyUserIfUpdateAvailable(cancellationToken);
        }
    }
}
