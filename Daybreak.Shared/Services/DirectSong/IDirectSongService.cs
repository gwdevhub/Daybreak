using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Mods;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.DirectSong;
public interface IDirectSongService : IModService
{
    DirectSongInstallationStatus? CachedInstallationStatus { get; }
    Task<bool>? InstallationTask { get; }
    Task<bool> SetupDirectSong(DirectSongInstallationStatus directSongInstallationStatus, CancellationToken cancellationToken);
}
