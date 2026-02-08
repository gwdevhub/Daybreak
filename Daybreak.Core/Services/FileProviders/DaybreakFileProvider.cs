using System.Extensions.Core;
using Daybreak.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Daybreak.Services.FileProviders;

internal sealed class DaybreakFileProvider : IFileProvider
{
    private const string WwwrootPrefix = "wwwroot/";
    private const string ContentPrefix = "_content/Daybreak.Core/";

    private readonly Dictionary<string, (string OriginalKey, FileProviderAssembly Provider)>
        manifestMapping = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILogger<DaybreakFileProvider> logger;

    public DaybreakFileProvider(
        IEnumerable<FileProviderAssembly> fileProviderAssemblies,
        ILogger<DaybreakFileProvider> logger
    )
    {
        this.logger = logger;

        foreach (var provider in fileProviderAssemblies)
        {
            var manifestNames = provider.Assembly.GetManifestResourceNames();
            foreach (var name in manifestNames)
            {
                // Store with original key preserved for resource loading
                this.manifestMapping[name] = (name, provider);
                
                // Normalize to forward slashes
                var normalizedName = name.Replace("\\", "/");
                if (normalizedName != name && !this.manifestMapping.ContainsKey(normalizedName))
                {
                    this.manifestMapping[normalizedName] = (name, provider);
                }
                
                // Also register without wwwroot/ prefix for runtime lookups
                if (normalizedName.StartsWith(WwwrootPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var withoutWwwroot = normalizedName.Substring(WwwrootPrefix.Length);
                    if (!this.manifestMapping.ContainsKey(withoutWwwroot))
                    {
                        this.manifestMapping[withoutWwwroot] = (name, provider);
                    }
                }
            }
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        // Directory enumeration from embedded resources is not fully supported
        return NotFoundDirectoryContents.Singleton;
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        // Normalize to forward slashes for cross-platform lookup
        var normalizedKey = subpath.Replace("\\", "/").TrimStart('/');

        // Also try with backslashes for Windows-built embedded resources
        var windowsKey = subpath.Replace("/", "\\").TrimStart('\\');

        // Try embedded resources directly (case-insensitive due to dictionary comparer)
        if (
            this.manifestMapping.TryGetValue(normalizedKey, out var match)
            || this.manifestMapping.TryGetValue(windowsKey, out match)
        )
        {
            var name = Path.GetFileName(subpath);
            scopedLogger.LogDebug(
                "Serving embedded resource: {Path} from assembly {Assembly}",
                normalizedKey,
                match.Provider.Assembly.GetName().Name ?? string.Empty);
            return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                match.Provider.Assembly,
                match.OriginalKey,
                name,
                DateTime.UtcNow
            );
        }

        // Try with wwwroot/ prefix for files requested without it
        if (!normalizedKey.StartsWith(WwwrootPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var wwwrootKey = WwwrootPrefix + normalizedKey;
            if (this.manifestMapping.TryGetValue(wwwrootKey, out match))
            {
                var name = Path.GetFileName(subpath);
                scopedLogger.LogDebug(
                    "Serving embedded resource (wwwroot): {Path} from assembly {Assembly}",
                    wwwrootKey,
                    match.Provider.Assembly.GetName().Name ?? string.Empty);
                return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                    match.Provider.Assembly,
                    match.OriginalKey,
                    name,
                    DateTime.UtcNow
                );
            }
        }

        // RCL assets from Daybreak.Core land under _content/Daybreak.Core/ in requests.
        // Strip that prefix and try wwwroot/ instead.
        if (normalizedKey.StartsWith(ContentPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var strippedKey = normalizedKey.Substring(ContentPrefix.Length);
            return this.GetFileInfo(strippedKey);
        }

        scopedLogger.LogWarning("File not found: {Path}", subpath);
        return new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
