using Daybreak.Controls;
using Daybreak.Models.Progress;
using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve
{
    public interface IIconDownloader
    {
        void SetBrowser(WebView2 chromiumBrowserWrapper);

        bool DownloadComplete { get; }
        Task<IconDownloadStatus> StartIconDownload();
        void CancelIconDownload();
    }
}
