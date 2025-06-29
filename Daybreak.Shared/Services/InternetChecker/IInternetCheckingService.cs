using Daybreak.Shared.Models;

namespace Daybreak.Shared.Services.InternetChecker;

public interface IInternetCheckingService
{
    Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken);
}
