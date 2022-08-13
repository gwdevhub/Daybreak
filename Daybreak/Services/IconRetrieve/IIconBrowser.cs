using Daybreak.Controls;
using Daybreak.Models;
using System.Threading;

namespace Daybreak.Services.IconRetrieve
{
    public interface IIconBrowser
    {
        void InitializeWebView(ChromiumBrowserWrapper webView2, CancellationToken cancellationToken);
        /// <summary>
        /// Queue an icon request. The browser will attempt to download the icon. Monitor the <see cref="IconRequest.Finished"/> to be notified when the request has been served.
        /// </summary>
        /// <param name="iconRequest">Request model.</param>
        void QueueIconRequest(IconRequest iconRequest);
    }
}
