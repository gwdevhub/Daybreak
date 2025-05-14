namespace Daybreak.Shared.Services.Options;

public interface IOptionsProducer
{
    void RegisterOptions<T>()
        where T : new();
}
