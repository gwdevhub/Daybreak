using Microsoft.Extensions.Options;

namespace Daybreak.Services.Options;

internal sealed class DaybreakOptionsFactory<TOptions>(OptionsManager optionsManager) : IOptionsFactory<TOptions>
    where TOptions : class
{
    private readonly OptionsManager optionsManager = optionsManager;

    public TOptions Create(string name)
    {
        return this.optionsManager.GetOptions<TOptions>();
    }
}
