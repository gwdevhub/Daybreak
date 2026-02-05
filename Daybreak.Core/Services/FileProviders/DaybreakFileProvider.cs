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
                // Store both forward and backslash versions for cross-platform compatibility
                this.manifestMapping[name] = provider;
                // Normalize to forward slashes as well
                var normalizedName = name.Replace("\\", "/");
                if (normalizedName != name)
                {
                    this.manifestMapping[normalizedName] = provider;
                }
            }
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = Path.Combine(WwwrootPath, subpath.TrimStart('/'));
        if (Directory.Exists(path))
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalDirectoryInfo(new DirectoryInfo(path));
        }
        
        return NotFoundDirectoryContents.Singleton;
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        // Normalize to forward slashes for cross-platform lookup
        var normalizedKey = subpath.Replace("\\", "/");
        // Also try with backslashes for Windows-built embedded resources
        var windowsKey = subpath.Replace("/", "\\");
        
        // Try normalized key first, then Windows key
        if (this.manifestMapping.TryGetValue(normalizedKey, out var provider) ||
            this.manifestMapping.TryGetValue(windowsKey, out provider))
        {
            var name = Path.GetFileName(subpath);
            // Use the key format that matched
            var resourceKey = this.manifestMapping.ContainsKey(normalizedKey) ? normalizedKey : windowsKey;
            return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                provider.Assembly,
                resourceKey,
                name,
                DateTime.UtcNow);
        }

        var path = Path.Combine(WwwrootPath, subpath.TrimStart('/'));
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
