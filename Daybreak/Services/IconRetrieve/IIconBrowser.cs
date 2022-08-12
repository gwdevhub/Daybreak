using Microsoft.Web.WebView2.Wpf;
using Models;
using System.Threading;

namespace Services.IconRetrieve
{
    public interface IIconBrowser
    {
        void InitializeWebView(WebView2 webView2, CancellationToken cancellationToken);
        /// <summary>
        /// Queue an icon request. The browser will attempt to download the icon. Monitor the <see cref="IconRequest.Finished"/> to be notified when the request has been served.
        /// </summary>
        /// <param name="iconRequest">Request model.</param>
        void QueueIconRequest(IconRequest iconRequest);
    }
}
