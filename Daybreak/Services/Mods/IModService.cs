using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Daybreak.Services.Mods;

public interface IModService
{
    bool IsEnabled { get; set; }
    bool IsInstalled { get; }
    IEnumerable<string> GetCustomArguments();
    Task OnGuildwarsStarting(Process process);
    Task OnGuildwarsStarted(Process process);
}
