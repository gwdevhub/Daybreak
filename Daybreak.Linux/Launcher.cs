using Daybreak.Launch;
using Daybreak.Linux.Configuration;

namespace Daybreak.Linux;

public static partial class Launcher
{
    public static void Main(string[] args)
    {
        var bootstrap = Daybreak.Launch.Launcher.SetupBootstrap();
        var platformConfiguration = new LinuxPlatformConfiguration();
        Daybreak.Launch.Launcher.LaunchSequence(args, bootstrap, platformConfiguration);
    }
}
