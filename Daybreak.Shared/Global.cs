using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Core")]
[assembly: InternalsVisibleTo("Daybreak.Windows")]
[assembly: InternalsVisibleTo("Daybreak.Linux")]

namespace Daybreak.Shared;

public static class Global
{
    //Will get set by Daybreak on application startup
    public static IServiceProvider GlobalServiceProvider { get; internal set; } = default!;
}
