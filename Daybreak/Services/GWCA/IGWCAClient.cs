using Daybreak.Models.GWCA;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;
public interface IGWCAClient
{
    Task<ConnectionContext?> Connect(Process process, CancellationToken cancellationToken);

    Task<bool> CheckAlive(ConnectionContext connectionContext, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAsync(ConnectionContext connectionContext, string subPath, CancellationToken cancellationToken);
}
