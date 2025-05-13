using Daybreak.Models.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Guildwars;

public interface IGuildWarsCopyService
{
    Task CopyGuildwars(string existingExecutable, CopyStatus copyStatus, CancellationToken cancellationToken);
}
