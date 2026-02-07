using System.Extensions.Core;
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
        this.LoadStaticWebAssets();
    }

    private void LoadStaticWebAssets()
    {
        var appDir = PathUtils.GetRootFolder();

        // Try loading from runtime JSON first (used during development/debug builds)
        this.LoadStaticWebAssetsFromRuntimeJson(appDir);

        // Also load from endpoints JSON (used in publish builds where files are under wwwroot/)
        this.LoadStaticWebAssetsFromEndpointsJson(appDir);
    }

    private void LoadStaticWebAssetsFromRuntimeJson(string appDir)
    {
        try
        {
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
                        this.ParseStaticWebAssetsTree(rootElement, "", roots);
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
            this.logger.LogWarning(ex, "Failed to load static web assets runtime manifest");
        }
    }

    private void LoadStaticWebAssetsFromEndpointsJson(string appDir)
    {
        try
        {
            var endpointsJsonFiles = Directory.GetFiles(
                appDir,
                "*.staticwebassets.endpoints.json"
            );

            foreach (var jsonFile in endpointsJsonFiles)
            {
                try
                {
                    var jsonContent = File.ReadAllText(jsonFile);
                    using var doc = JsonDocument.Parse(jsonContent);
                    var countBefore = this.staticWebAssetsMapping.Count;

                    // The endpoints JSON may be a bare array or an object with
                    // { "Version": ..., "Endpoints": [...] } depending on SDK version.
                    JsonElement endpointsArray;
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        endpointsArray = doc.RootElement;
                    }
                    else if (
                        doc.RootElement.ValueKind == JsonValueKind.Object
                        && doc.RootElement.TryGetProperty("Endpoints", out var endpointsProp)
                        && endpointsProp.ValueKind == JsonValueKind.Array
                    )
                    {
                        endpointsArray = endpointsProp;
                    }
                    else
                    {
                        continue;
                    }

                    foreach (var endpoint in endpointsArray.EnumerateArray())
                    {
                        if (
                            !endpoint.TryGetProperty("Route", out var routeElement)
                            || !endpoint.TryGetProperty("AssetFile", out var assetFileElement)
                        )
                            continue;

                        var route = routeElement.GetString();
                        var assetFile = assetFileElement.GetString();
                        if (string.IsNullOrEmpty(route) || string.IsNullOrEmpty(assetFile))
                            continue;

                        // Skip compressed variants
                        if (assetFile.EndsWith(".br") || assetFile.EndsWith(".gz"))
                            continue;

                        // Skip fingerprinted routes (they contain a hash segment)
                        // We only want the clean route names
                        if (this.staticWebAssetsMapping.ContainsKey(route))
                            continue;

                        // Resolve the asset file path relative to wwwroot
                        var physicalPath = Path.Combine(WwwrootPath, assetFile);
                        if (File.Exists(physicalPath))
                        {
                            this.staticWebAssetsMapping[route] = physicalPath;
                        }
                    }

                    this.logger.LogDebug(
                        "Loaded {Count} static web asset endpoints from {File}",
                        this.staticWebAssetsMapping.Count - countBefore,
                        Path.GetFileName(jsonFile)
                    );
                }
                catch (Exception ex)
                {
                    this.logger.LogWarning(
                        ex,
                        "Failed to parse static web asset endpoints from {File}",
                        jsonFile
                    );
                }
            }
        }
        catch (Exception ex)
        {
            this.logger.LogWarning(ex, "Failed to load static web assets endpoints manifest");
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
                this.ParseStaticWebAssetsTree(child.Value, childPath, contentRoots);
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
        var scopedLogger = this.logger.CreateScopedLogger();
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
            scopedLogger.LogDebug("Serving static web asset: {Path}", normalizedKey);
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
            scopedLogger.LogDebug(
                "Serving embedded resource: {Path} from assembly {Assembly}",
                normalizedKey,
                match.Provider.Assembly.GetName().Name ?? string.Empty);
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
            scopedLogger.LogDebug("Serving physical file: {Path}", path);
            return new Microsoft.Extensions.FileProviders.Physical.PhysicalFileInfo(
                new FileInfo(path)
            );
        }

        // RCL assets from Daybreak.Core land under _content/Daybreak.Core/ in publish output.
        // Retry the lookup with that prefix so bare requests (e.g. css/site.css, js/*.js) resolve.
        const string coreContentPrefix = "_content/Daybreak.Core/";
        if (!normalizedKey.StartsWith(coreContentPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return this.GetFileInfo(coreContentPrefix + normalizedKey);
        }

        scopedLogger.LogWarning("File not found: {Path}", subpath);
        return new NotFoundFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }
}
