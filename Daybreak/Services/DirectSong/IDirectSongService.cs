using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.DirectSong;
public interface IDirectSongService : IModService
{
    DirectSongInstallationStatus? CachedInstallationStatus { get; }
    Task<bool>? InstallationTask { get; }
    Task<bool> SetupDirectSong(DirectSongInstallationStatus directSongInstallationStatus, CancellationToken cancellationToken);
}
