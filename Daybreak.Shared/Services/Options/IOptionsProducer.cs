namespace Daybreak.Services.Options;

public interface IOptionsProducer
{
    void RegisterOptions<T>()
        where T : new();
}
