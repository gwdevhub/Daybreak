using Daybreak.Shared.Models.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Guildwars;

public interface IGuildWarsCopyService
{
    Task CopyGuildwars(string existingExecutable, CopyStatus copyStatus, CancellationToken cancellationToken);
}
