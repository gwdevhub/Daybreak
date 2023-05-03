using System.IO;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModWikiView.xaml
/// </summary>
public partial class UModBrowserView : UserControl
{
    private const string WhitelistedExtension = ".tpf";

    public UModBrowserView()
    {
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
}
