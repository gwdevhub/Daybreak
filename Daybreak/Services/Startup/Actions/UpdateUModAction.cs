using Daybreak.Shared.Models;
using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.UMod;
using System.Core.Extensions;

namespace Daybreak.Services.Startup.Actions;
internal sealed class UpdateUModAction(
    IUModService uModService) : StartupActionBase
{
    private readonly IUModService uModService = uModService.ThrowIfNull();

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        var progress = new Progress<ProgressUpdate>();
        await this.uModService.CheckAndUpdateUMod(progress, cancellationToken);
    }
}
