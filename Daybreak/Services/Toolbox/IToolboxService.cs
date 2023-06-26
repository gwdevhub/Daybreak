using Daybreak.Models.Progress;
using Daybreak.Services.Mods;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;
public interface IToolboxService : IModService
{
    bool LoadToolboxFromDisk();

    Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus);
}
