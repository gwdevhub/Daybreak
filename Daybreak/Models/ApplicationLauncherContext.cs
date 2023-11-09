using System.Diagnostics;

namespace Daybreak.Models;
public readonly struct ApplicationLauncherContext
{
    public string ExecutablePath { get; init; }
    public Process Process { get; init; }
}
