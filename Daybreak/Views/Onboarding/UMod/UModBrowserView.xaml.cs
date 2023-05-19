using Daybreak.Configuration.Options;
using Daybreak.Services.UMod;
using System.Configuration;
using System.Core.Extensions;
using System.IO;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModWikiView.xaml
/// </summary>
public partial class UModBrowserView : UserControl
{
    private const string WhitelistedExtension = ".tpf";

    private readonly IUModService uModService;
    private readonly ILiveOptions<UModOptions> liveOptions;

    public UModBrowserView(
        IUModService uModService,
        ILiveOptions<UModOptions> liveOptions)
    {
        this.uModService = uModService.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();

        this.InitializeComponent();
    }

    private void ChromiumBrowserWrapper_DownloadingFile(object _, Models.Browser.DownloadPayload e)
    {
        /*
         * According to https://wiki.guildwars.com/wiki/Player-made_Modifications#Shared_player_content,
         * download links must link directly to a .tpf file. Only allow downloads for .tpf files.
         */

        var path = e.ResultingFilePath;
        var extension = Path.GetExtension(path);
        if (extension?.Equals(WhitelistedExtension, System.StringComparison.OrdinalIgnoreCase) is true)
        {
            return;
        }

        e.CanDownload = false;
        return;
    }

    private void ChromiumBrowserWrapper_DownloadedFile(object _, string e)
    {
        /*
         * A tpf file has been downloaded.
         * Automatically add it to the managed mods list.
         */

        if (this.liveOptions.Value.AutoEnableMods)
        {
            this.uModService.AddMod(e);
        }
    }
}
