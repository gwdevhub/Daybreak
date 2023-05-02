using Daybreak.Models.Progress;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod;
public interface IUModService
{
    bool UModExists { get; }

    bool Enabled { get; }

    Task<bool> SetupUMod(UModInstallationStatus uModInstallationStatus);
}
