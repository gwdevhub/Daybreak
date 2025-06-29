using Daybreak.Configuration.Options;
using Daybreak.Shared.Models;
using System.Configuration;
using System.Core.Extensions;

namespace Daybreak.Services.Startup.Actions;
public sealed class BrowserHistorySizeEnforcer(
    IUpdateableOptions<FocusViewOptions> liveOptions) : StartupActionBase
{
    private readonly IUpdateableOptions<FocusViewOptions> liveOptions = liveOptions.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        while(this.liveOptions.Value.BrowserHistory.History.Count > 20)
        {
            this.liveOptions.Value.BrowserHistory.History.RemoveAt(0);
            this.liveOptions.Value.BrowserHistory.CurrentPosition--;
        }

        if (this.liveOptions.Value.BrowserHistory.CurrentPosition < 0)
        {
            this.liveOptions.Value.BrowserHistory.CurrentPosition = 0;
        }

        this.liveOptions.UpdateOption();
    }
}
