using Daybreak.Shared.Models.Progress;

namespace Daybreak.Shared.Services.Guildwars;

public interface IGuildWarsCopyService
{
    Task CopyGuildwars(string existingExecutable, CopyStatus copyStatus, CancellationToken cancellationToken);
}
