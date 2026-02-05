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
    // Use case-insensitive dictionary for cross-platform compatibility (Linux is case-sensitive)
    private readonly Dictionary<string, (string OriginalKey, FileProviderAssembly Provider)> manifestMapping = 
        new(StringComparer.OrdinalIgnoreCase);
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
                // Store with original key preserved for resource loading
                this.manifestMapping[name] = (name, provider);
                // Also store normalized forward-slash version
                var normalizedName = name.Replace("\\", "/");
                if (normalizedName != name && !this.manifestMapping.ContainsKey(normalizedName))
                {
                    this.manifestMapping[normalizedName] = (name, provider);
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
        var normalizedKey = subpath.Replace("\\", "/").TrimStart('/');
        // Also try with backslashes for Windows-built embedded resources
        var windowsKey = subpath.Replace("/", "\\").TrimStart('\\');
        
        // Try normalized key first, then Windows key (case-insensitive due to dictionary comparer)
        if (this.manifestMapping.TryGetValue(normalizedKey, out var match) ||
            this.manifestMapping.TryGetValue(windowsKey, out match))
        {
            var name = Path.GetFileName(subpath);
            // Use the original key that was registered (preserves case for resource loading)
            return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                match.Provider.Assembly,
                match.OriginalKey,
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
