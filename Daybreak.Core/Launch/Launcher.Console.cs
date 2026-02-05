using Daybreak.Shared.Utils;

namespace Daybreak.Launch;

public partial class Launcher
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
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
