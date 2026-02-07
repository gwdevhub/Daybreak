using Daybreak.Linux.Configuration;

namespace Daybreak.Linux;

public static partial class Launcher
{
    public static void Main(string[] args)
    {
        var bootstrap = Launch.Launcher.SetupBootstrap();
        var platformConfiguration = new LinuxPlatformConfiguration();
        Launch.Launcher.LaunchSequence(args, bootstrap, platformConfiguration);
    }
}
