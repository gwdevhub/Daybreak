using Daybreak.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.InternetChecker;

internal interface IInternetCheckingService
{
    Task<InternetConnectionState> CheckConnectionAvailable(CancellationToken cancellationToken);
}
