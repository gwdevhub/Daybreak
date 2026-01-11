using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;
internal sealed class UpdateUModAction(
    IUModService uModService,
    ILogger<UpdateUModAction> logger) : StartupActionBase
{
    private readonly IUModService uModService = uModService.ThrowIfNull();
    private readonly ILogger<UpdateUModAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var progress = new Progress<ProgressUpdate>();
        if (!this.uModService.IsInstalled)
        {
            scopedLogger.LogInformation("UMod is not installed. Skipping update.");
            return;
        }

        scopedLogger.LogInformation("Checking for UMod updates...");
        Task.Factory.StartNew(async () => await this.uModService.CheckAndUpdateUMod(progress, CancellationToken.None));
        
    }
}
