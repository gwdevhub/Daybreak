using Daybreak.Shared.Models;
using Daybreak.Shared.Services.UMod;
using System.Core.Extensions;

namespace Daybreak.Services.Startup.Actions;
internal sealed class UpdateUModAction : StartupActionBase
{
    private readonly IUModService uModService;

    public UpdateUModAction(
        IUModService uModService)
    {
        this.uModService = uModService.ThrowIfNull();
    }

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        await this.uModService.CheckAndUpdateUMod(cancellationToken);
    }
}
