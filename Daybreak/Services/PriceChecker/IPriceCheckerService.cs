using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.PriceChecker;
public interface IPriceCheckerService
{
    Task CheckForPrices(CancellationToken cancellationToken);
}
