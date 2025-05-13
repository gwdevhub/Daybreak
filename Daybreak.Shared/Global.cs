using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Daybreak")]

namespace Daybreak.Shared;

public static class Global
{
    public static IServiceProvider GlobalServiceProvider { get; internal set; } = default!;
}
