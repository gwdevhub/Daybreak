using Daybreak.Windows.Configuration;
using System.Runtime.InteropServices;

namespace Daybreak.Windows;

internal sealed partial class Launcher
{
#if DEBUG
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();
#endif

    [STAThread]
    private static void Main(string[] args)
    {
#if DEBUG
        AllocConsole();
#endif
        var bootstrap = Daybreak.Launch.Launcher.SetupBootstrap();
        var platformConfiguration = new WindowsPlatformConfiguration();
        Daybreak.Launch.Launcher.LaunchSequence(args, bootstrap, platformConfiguration);
    }
}
