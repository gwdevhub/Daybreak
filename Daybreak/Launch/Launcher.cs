using Daybreak.Shared.Utils;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Tests")]

namespace Daybreak.Launch;

public static class Launcher
{
    //public static WpfLauncher Instance { get; private set; } = default!;

    [STAThread]
    public static int Main(string[] args)
    {
#if DEBUG
        AllocateAnsiConsole();
#endif

        var splashWindow = new SplashWindow(args);
        splashWindow.SplashWindowApp.Run();
        return 0;
    }

    private static void AllocateAnsiConsole()
    {
        NativeMethods.AllocConsole();
        var handle = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);
        if (!NativeMethods.GetConsoleMode(handle, out var mode))
        {
            Console.WriteLine("Failed to get console mode");
        }

        if (!NativeMethods.SetConsoleMode(handle, mode | NativeMethods.ENABLE_VIRTUAL_TERMINAL_PROCESSING | NativeMethods.ENABLE_PROCESSED_OUTPUT))
        {
            Console.WriteLine("Failed to enable virtual terminal processing");
        }
    }
}
