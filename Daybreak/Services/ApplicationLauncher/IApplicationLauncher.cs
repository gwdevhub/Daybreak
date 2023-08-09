using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Services.ApplicationLauncher;

public interface IApplicationLauncher
{
    bool IsGuildwarsRunning { get; }
    Process? RunningGuildwarsProcess { get; }
    Task<Process?> LaunchGuildwars();
    void RestartDaybreakAsAdmin();
    void RestartDaybreakAsNormalUser();
}
