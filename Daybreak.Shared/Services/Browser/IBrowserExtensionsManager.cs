using Microsoft.Web.WebView2.Core;
using System.Threading.Tasks;

namespace Daybreak.Services.Browser;

public interface IBrowserExtensionsManager
{
    Task InitializeBrowserEnvironment(CoreWebView2Profile coreWebView2Profile, string browserVersion);
}
