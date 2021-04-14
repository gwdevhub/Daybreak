using System.Threading.Tasks;

namespace Daybreak.Services.ApplicationLauncher
{
    public interface IApplicationLauncher
    {
        bool IsGuildwarsRunning { get; }
        bool IsToolboxRunning { get; }
        Task LaunchGuildwars();
        Task LaunchGuildwarsToolbox();
    }
}
