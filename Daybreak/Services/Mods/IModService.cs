using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Services.Mods;

public interface IModService
{
    bool IsEnabled { get; set; }
    bool IsInstalled { get; }
    IEnumerable<string> GetCustomArguments();
    /// <summary>
    /// Called before starting the guild wars process.
    /// Do mod preparation here.
    /// </summary>
    Task OnGuildwarsStarting(Process process);
    /// <summary>
    /// Called when the process is created in suspended state.
    /// Do dll injection here.
    /// </summary>
    Task OnGuildWarsCreated(Process process);
    /// <summary>
    /// Called after the process has been resumed.
    /// Do clean-up/integration with the guild wars process here.
    /// </summary>
    Task OnGuildwarsStarted(Process process);
}
