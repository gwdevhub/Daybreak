using Daybreak.Shared.Models;
using Daybreak.Shared.Services.UMod;
using System.Core.Extensions;

namespace Daybreak.Services.Startup.Actions;
internal sealed class UpdateUModAction(
    IUModService uModService) : StartupActionBase
{
    private readonly IUModService uModService = uModService.ThrowIfNull();

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        await this.uModService.CheckAndUpdateUMod(cancellationToken);
    }
}
