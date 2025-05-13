using Daybreak.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.InternetChecker;

public interface IInternetCheckingService
{
    Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken);
}
