using Microsoft.Web.WebView2.Core;

namespace Daybreak.Shared.Services.Browser;

public interface IBrowserExtensionsManager
{
    Task InitializeBrowserEnvironment(CoreWebView2Profile coreWebView2Profile, string browserVersion);
}
