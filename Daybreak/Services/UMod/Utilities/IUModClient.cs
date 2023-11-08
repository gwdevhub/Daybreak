using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Daybreak.Services.UMod.Models;

namespace Daybreak.Services.UMod.Utilities;

public interface IUModClient
{
    Task<UModConnectionContext?> Initialize(Process process, CancellationToken cancellationToken);
    Task AddFile(string filePath, UModConnectionContext uModConnectionContext, CancellationToken cancellationToken);
}
