using Daybreak.Shared.Models.Browser;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.UMod;
using Daybreak.Utils;
using System.Core.Extensions;
using System.IO;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModWikiView.xaml
/// </summary>
public partial class UModBrowserView : UserControl
{
    private static readonly string[] WhitelistedExtensions = [".tpf", ".zip"];

    private readonly INotificationService notificationService;
    private readonly IUModService uModService;

    private DateTime lastDownloadingNotificationTime = DateTime.Now;
    private DateTime lastDownloadedNotificationTime = DateTime.Now;

    public UModBrowserView(
        INotificationService notificationService,
        IUModService uModService)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.uModService = uModService.ThrowIfNull();

        this.InitializeComponent();
    }

    private async void ChromiumBrowserWrapper_DownloadingFile(object? _, DownloadPayload e)
    {
        /*
         * According to https://wiki.guildwars.com/wiki/Player-made_Modifications#Shared_player_content,
         * download links must link directly to a .zip or .tpf file. Only allow downloads for .zip or .tpf files.
         */

        var path = e.ResultingFilePath;
        var extension = Path.GetExtension(path);
        if (WhitelistedExtensions.Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase)))
        {
            // TODO: #378 Notification deduplication logic. This is due to a bug in WebView2 where DownloadStarting is triggered twice. Remove once event is fixed
            if ((DateTime.Now - this.lastDownloadingNotificationTime).TotalMilliseconds > 100)
            {
                this.lastDownloadingNotificationTime = DateTime.Now;
                this.notificationService.NotifyInformation(
                    title: $"Downloading {Path.GetFileName(e.ResultingFilePath)}",
                    description: $"Downloading uMod mod to {e.ResultingFilePath}");
                await this.Browser.WebBrowser.ExecuteScriptAsync(Scripts.CreateAlert($"Downloading {Path.GetFileName(e.ResultingFilePath)}"));
            }

            return;
        }

        e.CanDownload = false;
        return;
    }

    private async void ChromiumBrowserWrapper_DownloadedFile(object? _, string e)
    {
        /*
         * A tpf file has been downloaded.
         * Automatically add it to the managed mods list.
         */
        if ((DateTime.Now - this.lastDownloadedNotificationTime).TotalMilliseconds > 100)
        {
            // TODO: #378 Notification deduplication logic. This is due to a bug in WebView2 where DownloadStarting is triggered twice. Remove once event is fixed
            this.lastDownloadedNotificationTime = DateTime.Now;
            this.notificationService.NotifyInformation(
                title: $"Downloaded {Path.GetFileName(e)}",
                description: $"Downloaded mod and added to uMod list");
            await this.Browser.WebBrowser.ExecuteScriptAsync(Scripts.CreateAlert($"Downloaded {Path.GetFileName(e)}"));
        }

        this.uModService.AddMod(e);
    }
}
