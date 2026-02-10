using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak.Core")]
[assembly: InternalsVisibleTo("Daybreak.Windows")]
[assembly: InternalsVisibleTo("Daybreak.Linux")]

namespace Daybreak.Shared;

public static class Global
{
    public static IServiceProvider GlobalServiceProvider { get; internal set; } = default!;

    public static DateTime NotificationShortExpiration => DateTime.UtcNow + TimeSpan.FromSeconds(15);
    public static DateTime NotificationLongExpiration => DateTime.UtcNow + TimeSpan.FromMinutes(1);
    public static DateTime NotificationNeverExpire => DateTime.MaxValue;
}
