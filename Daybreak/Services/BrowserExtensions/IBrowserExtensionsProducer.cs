namespace Daybreak.Services.BrowserExtensions;
public interface IBrowserExtensionsProducer
{
    void RegisterExtension<T>()
        where T : class, IBrowserExtension;
}
