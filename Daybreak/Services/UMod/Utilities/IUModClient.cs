using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod.Utilities;
public interface IUModClient : IDisposable
{
    bool Ready { get; }
    void Initialize(CancellationToken cancellationToken);
    Task WaitForInitialize(CancellationToken cancellationToken);
    void CloseConnection();
    Task AddFile(string filePath, CancellationToken cancellationToken);
    Task<bool> Send(CancellationToken cancellationToken);
}
