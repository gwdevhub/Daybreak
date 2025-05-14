using System.Diagnostics;

namespace Daybreak.Shared.Models;
public readonly struct ApplicationLauncherContext
{
    public string ExecutablePath { get; init; }
    public Process Process { get; init; }
    public uint ProcessId { get; init; }
}
