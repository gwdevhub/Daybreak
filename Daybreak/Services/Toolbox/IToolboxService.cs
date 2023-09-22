using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;
public interface IToolboxService : IModService
{
    bool LoadToolboxFromDisk();

    bool LoadToolboxFromUsualLocation();

    Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus);
}
