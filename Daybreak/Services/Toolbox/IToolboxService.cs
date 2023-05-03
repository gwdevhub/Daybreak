using Daybreak.Models.Progress;
using System.Threading.Tasks;

namespace Daybreak.Services.Toolbox;
public interface IToolboxService
{
    bool ToolboxExists { get; }

    bool Enabled { get; set; }

    bool LoadToolboxFromDisk();

    Task<bool> SetupToolbox(ToolboxInstallationStatus toolboxInstallationStatus);
}
