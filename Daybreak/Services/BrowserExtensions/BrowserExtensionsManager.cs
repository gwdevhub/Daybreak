using Microsoft.Web.WebView2.Core;
using Slim;
using System.Core.Extensions;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.BrowserExtensions;

public sealed class BrowserExtensionsManager : IBrowserExtensionsManager, IBrowserExtensionsProducer
{
    private readonly IServiceManager serviceManager;

    public BrowserExtensionsManager(
        IServiceManager serviceManager)
    {
        this.serviceManager = serviceManager.ThrowIfNull();
    }

    public void RegisterExtension<T>()
        where T : class, IBrowserExtension
    {
        this.serviceManager.RegisterScoped<T, T>();
    }

    public async Task InitializeBrowserEnvironment(CoreWebView2Profile coreWebView2Profile, string browserVersion)
    {
        var existingExtensions = await coreWebView2Profile.GetBrowserExtensionsAsync();
        foreach (var extension in this.serviceManager.GetServicesOfType<IBrowserExtension>()
            .Where(e => existingExtensions.None(ee => ee.Id == e.ExtensionId)))
        {
            await extension.CheckAndUpdate(browserVersion);
            var extensionPath = await extension.GetExtensionPath();
            if (!Directory.Exists(extensionPath))
            {
                continue;
            }

            if (!File.Exists(Path.Combine(extensionPath, "manifest.json")))
            {
                continue;
            }

            await coreWebView2Profile.AddBrowserExtensionAsync(extensionPath);
        }
    }
}
