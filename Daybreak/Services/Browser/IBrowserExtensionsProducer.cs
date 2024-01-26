namespace Daybreak.Services.Browser;
public interface IBrowserExtensionsProducer
{
    void RegisterExtension<T>()
        where T : class, IBrowserExtension;
}
