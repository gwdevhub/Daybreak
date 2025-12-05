using Daybreak.Shared.Models.Async;

namespace Daybreak.Shared.Services.Guildwars;

public interface IGuildWarsCopyService
{
    ProgressAsyncOperation<bool> CopyGuildwars(string existingExecutable, CancellationToken cancellationToken);
}
