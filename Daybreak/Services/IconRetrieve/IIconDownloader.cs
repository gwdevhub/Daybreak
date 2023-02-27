using Daybreak.Controls;
using Daybreak.Models.Progress;
using System.Threading.Tasks;

namespace Daybreak.Services.IconRetrieve;

public interface IIconDownloader
{
    void SetBrowser(ChromiumBrowserWrapper chromiumBrowserWrapper);

    bool DownloadComplete { get; }
    Task<IconDownloadStatus> StartIconDownload();
    void CancelIconDownload();
}
