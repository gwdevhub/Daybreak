using Daybreak.Shared.Services.Options;

namespace Daybreak.Services.Options;

public sealed class OptionProducer : IOptionsProducer
{
    public List<Type> RegisteredOptions { get; } = [];

    public void RegisterOptions<TOptions>()
        where TOptions : class, new()
    {
        this.RegisteredOptions.Add(typeof(TOptions));
    }
}
