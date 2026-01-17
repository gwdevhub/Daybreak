using System.Reflection;

namespace Daybreak.Shared.Services.FileProviders;

public interface IFileProviderProducer
{
    void RegisterAssembly(Assembly assembly);
}
