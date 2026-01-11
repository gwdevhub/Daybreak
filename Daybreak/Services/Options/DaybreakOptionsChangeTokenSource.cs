using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Daybreak.Services.Options;

internal sealed class DaybreakOptionsChangeTokenSource<TOptions> : IOptionsChangeTokenSource<TOptions>
    where TOptions : class
{
    private readonly OptionsManager optionsManager;
    private CancellationTokenSource cts = new();

    public DaybreakOptionsChangeTokenSource(OptionsManager optionsManager)
    {
        this.optionsManager = optionsManager;
        this.optionsManager.RegisterHook<TOptions>(this.OnOptionsChanged);
    }

    public string? Name => Microsoft.Extensions.Options.Options.DefaultName;

    public IChangeToken GetChangeToken()
    {
        return new CancellationChangeToken(this.cts.Token);
    }

    private void OnOptionsChanged()
    {
        var oldCts = this.cts;
        this.cts = new CancellationTokenSource();
        oldCts.Cancel();
        oldCts.Dispose();
    }
}
