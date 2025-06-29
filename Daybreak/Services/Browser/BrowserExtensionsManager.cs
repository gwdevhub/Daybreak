using Daybreak.Shared.Services.Browser;
using Microsoft.Web.WebView2.Core;
using Slim;
using System.Core.Extensions;
using System.Extensions;
using System.IO;

namespace Daybreak.Services.Browser;

public sealed class BrowserExtensionsManager(
    IServiceManager serviceManager) : IBrowserExtensionsManager, IBrowserExtensionsProducer
{
    private readonly IServiceManager serviceManager = serviceManager.ThrowIfNull();

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
