using System.Diagnostics;
using Daybreak.Shared.Models.LaunchConfigurations;

namespace Daybreak.Shared.Models;
public readonly struct ApplicationLauncherContext
{
    public string ExecutablePath { get; init; }
    public Process Process { get; init; }
    public uint ProcessId { get; init; }
    public LaunchConfigurationWithCredentials? LaunchConfiguration { get; init; }
}
