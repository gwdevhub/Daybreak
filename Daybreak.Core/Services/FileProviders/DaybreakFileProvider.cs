using System.Text.Json;
using Daybreak.Models;
using Daybreak.Shared.Utils;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Daybreak.Services.FileProviders;

internal sealed class DaybreakFileProvider : IFileProvider
{
    private static readonly string WwwrootPath = PathUtils.GetAbsolutePathFromRoot("wwwroot");

    private readonly Dictionary<string, (string OriginalKey, FileProviderAssembly Provider)>
        manifestMapping = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string>
        staticWebAssetsMapping = new(StringComparer.OrdinalIgnoreCase);
    private readonly IEnumerable<FileProviderAssembly> fileProviderAssemblies;
    private readonly ILogger<DaybreakFileProvider> logger;

    public DaybreakFileProvider(
        IEnumerable<FileProviderAssembly> fileProviderAssemblies,
        ILogger<DaybreakFileProvider> logger
    )
    {
        this.fileProviderAssemblies = fileProviderAssemblies;
        this.logger = logger;

        foreach (var provider in this.fileProviderAssemblies)
        {
            var manifestNames = provider.Assembly.GetManifestResourceNames();
            foreach (var name in manifestNames)
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

        // Load static web assets from runtime JSON
        LoadStaticWebAssets();
    }

    private void LoadStaticWebAssets()
    {
        try
        {
            // Look for staticwebassets.runtime.json files in the application directory
            var appDir = PathUtils.GetRootFolder();
            var runtimeJsonFiles = Directory.GetFiles(appDir, "*.staticwebassets.runtime.json");

            foreach (var jsonFile in runtimeJsonFiles)
            {
                try
                {
                    var jsonContent = File.ReadAllText(jsonFile);
                    using var doc = JsonDocument.Parse(jsonContent);

                    if (!doc.RootElement.TryGetProperty("ContentRoots", out var contentRoots))
                        continue;

                    var roots = new List<string>();
                    foreach (var root in contentRoots.EnumerateArray())
                    {
                        roots.Add(root.GetString() ?? string.Empty);
                    }

                    if (doc.RootElement.TryGetProperty("Root", out var rootElement))
                    {
                        ParseStaticWebAssetsTree(rootElement, "", roots);
                    }

                    this.logger.LogDebug(
                        "Loaded {Count} static web assets from {File}",
                        this.staticWebAssetsMapping.Count,
                        Path.GetFileName(jsonFile)
                    );
                }
                catch (Exception ex)
                {
                    this.logger.LogWarning(
                        ex,
                        "Failed to parse static web assets from {File}",
                        jsonFile
                    );
                }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to load static web assets manifest");
        }
    }

    private void ParseStaticWebAssetsTree(
        JsonElement element,
        string currentPath,
        List<string> contentRoots
    )
    {
        if (element.TryGetProperty("Asset", out var asset) && asset.ValueKind != JsonValueKind.Null)
        {
            if (
                asset.TryGetProperty("ContentRootIndex", out var indexElement)
                && asset.TryGetProperty("SubPath", out var subPathElement)
            )
            {
                var index = indexElement.GetInt32();
                var subPath = subPathElement.GetString() ?? string.Empty;

                if (index >= 0 && index < contentRoots.Count)
                {
                    var fullPath = Path.Combine(contentRoots[index], subPath);
                    var routePath = currentPath.TrimStart('/');
                    this.staticWebAssetsMapping[routePath] = fullPath;
                }
            }
        }

        if (
            element.TryGetProperty("Children", out var children)
            && children.ValueKind == JsonValueKind.Object
        )
        {
            foreach (var child in children.EnumerateObject())
            {
                var childPath = string.IsNullOrEmpty(currentPath)
                    ? child.Name
                    : $"{currentPath}/{child.Name}";
                ParseStaticWebAssetsTree(child.Value, childPath, contentRoots);
            }
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        var path = Path.Combine(WwwrootPath, subpath.TrimStart('/'));
        if (Directory.Exists(path))
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalDirectoryInfo(
                new DirectoryInfo(path)
            );
        }

        return NotFoundDirectoryContents.Singleton;
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        // Normalize to forward slashes for cross-platform lookup
        var normalizedKey = subpath.Replace("\\", "/").TrimStart('/');
        // Also try with backslashes for Windows-built embedded resources
        var windowsKey = subpath.Replace("/", "\\").TrimStart('\\');

        // First, check static web assets mapping (for scoped CSS, FluentUI, etc.)
        if (
            this.staticWebAssetsMapping.TryGetValue(normalizedKey, out var physicalPath)
            && File.Exists(physicalPath)
        )
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo(
                new FileInfo(physicalPath)
            );
        }

        // Try embedded resources (case-insensitive due to dictionary comparer)
        if (
            this.manifestMapping.TryGetValue(normalizedKey, out var match)
            || this.manifestMapping.TryGetValue(windowsKey, out match)
        )
        {
            var name = Path.GetFileName(subpath);
            // Use the original key that was registered (preserves case for resource loading)
            return new Microsoft.Extensions.FileProviders.Embedded.EmbeddedResourceFileInfo(
                match.Provider.Assembly,
                match.OriginalKey,
                name,
                DateTime.UtcNow
            );
        }

        // Fallback to physical wwwroot path
        var path = Path.Combine(WwwrootPath, subpath.TrimStart('/'));
        if (File.Exists(path))
        {
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo(
                new FileInfo(path)
            );
        }

        return new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
