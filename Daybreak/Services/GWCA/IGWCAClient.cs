using Daybreak.Models.GWCA;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;
public interface IGWCAClient
{
    Task<ConnectionContext?> Connect(uint processId, CancellationToken cancellationToken);

    Task<bool> CheckAlive(ConnectionContext connectionContext, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAsync(ConnectionContext connectionContext, string subPath, CancellationToken cancellationToken);
}
