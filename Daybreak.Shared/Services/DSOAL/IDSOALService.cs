using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DSOAL;

public interface IDSOALService : IModService
{
    void EnsureDSOALSymbolicLinkExists();
    ProgressAsyncOperation<bool> SetupDSOAL(CancellationToken cancellationToken);
}
