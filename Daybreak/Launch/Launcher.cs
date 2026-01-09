using Photino.Blazor;
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
        CreateAndShowSplash(args, LaunchSequence);
        return 0;
    }

    private static void LaunchSequence(string[] args, PhotinoBlazorApp splashApp)
    {
        var builder = CreateMainBuilder(args);
        LaunchState.UpdateProgress(LaunchState.LoadingOptions);
    }
}
