using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Services.ApplicationLauncher;

public interface IApplicationLauncher
{
    bool IsTexmodRunning { get; }
    bool IsGuildwarsRunning { get; }
    bool IsToolboxRunning { get; }
    Process? RunningGuildwarsProcess { get; }
    Task<Process?> LaunchGuildwars();
    Task LaunchGuildwarsToolbox();
    Task LaunchTexmod();
    void RestartDaybreakAsAdmin();
}
