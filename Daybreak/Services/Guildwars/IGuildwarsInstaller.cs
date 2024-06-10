using Daybreak.Models.Progress;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Guildwars;
public interface IGuildwarsInstaller
{
    Task<bool> InstallGuildwars(string destinationPath, GuildwarsInstallationStatus installationStatus, CancellationToken cancellationToken);
}
