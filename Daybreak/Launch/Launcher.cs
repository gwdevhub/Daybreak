using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Tests")]
namespace Daybreak.Launch;

public static partial class Launcher
{
    [STAThread]
    public static int Main(string[] args)
    {
#if DEBUG
        AllocateAnsiConsole();
#endif
        CreateAndShowSplash(args,
            (_, __) => LaunchSequence(args));
        return 0;
    }

    private static void LaunchSequence(string[] args)
    {
        var builder = CreateMainBuilder(args);
        
    }
}
