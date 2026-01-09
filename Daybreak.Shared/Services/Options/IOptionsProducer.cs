namespace Daybreak.Shared.Services.Options;

public interface IOptionsProducer
{
    void RegisterOptions<TOptions>()
        where TOptions : class, new();
}
