namespace Daybreak.Shared.Services.Initialization;

public interface IOptionsProducer
{
    void RegisterOptions<TOptions>()
        where TOptions : class, new();
}
