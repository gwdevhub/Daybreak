using Daybreak.Models;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Daybreak.Services.FileProviders;

internal sealed class DaybreakFileProvider
    : IFileProvider
{
    private static readonly string WwwrootPath = PathUtils.GetAbsolutePathFromRoot("wwwroot");
    private readonly Dictionary<string, FileProviderAssembly> manifestMapping = [];
    private readonly IEnumerable<FileProviderAssembly> fileProviderAssemblies;
    private readonly ILogger<DaybreakFileProvider> logger;

    public DaybreakFileProvider(
        IEnumerable<FileProviderAssembly> fileProviderAssemblies,
        ILogger<DaybreakFileProvider> logger)
    {
        this.fileProviderAssemblies = fileProviderAssemblies;
        this.logger = logger;

        foreach(var provider in this.fileProviderAssemblies)
        {
            var manifestNames = provider.Assembly.GetManifestResourceNames();
            foreach(var name in manifestNames)
            {
                this.manifestMapping[name] = provider;
            }
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = Path.Combine(WwwrootPath, subpath);
        if (Directory.Exists(path))
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalDirectoryInfo(new DirectoryInfo(path));
        }
        
        return NotFoundDirectoryContents.Singleton;
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        var key = subpath.Replace("/", "\\");
        if (this.manifestMapping.TryGetValue(key, out var provider))
        {
            var name = Path.GetFileName(subpath);
            return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                provider.Assembly,
                key,
                name,
                DateTime.UtcNow);
        }

        var path = Path.Combine(WwwrootPath, subpath);
        if (File.Exists(path))
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo(new FileInfo(path));
        }

        return new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
