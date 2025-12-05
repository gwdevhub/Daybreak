using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Services.Mods;

namespace Daybreak.Shared.Services.DirectSong;
public interface IDirectSongService : IModService
{
    Task<bool> SetupDirectSong(IProgress<ProgressUpdate> progress, CancellationToken cancellationToken);
}
