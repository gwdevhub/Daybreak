using Daybreak.Services.Navigation;
using Daybreak.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModSwitchView.xaml
/// </summary>
public partial class UModSwitchView : UserControl
{
    private const string WikiLink = "https://code.google.com/archive/p/texmod/wikis/uMod.wiki";
    private const string ModsLink = "https://wiki.guildwars.com/wiki/Player-made_Modifications#Shared_player_content";

    private readonly IUModService uModService;
    private readonly IViewManager viewManager;
    private readonly ILogger<UModSwitchView> logger;

    [GenerateDependencyProperty]
    private bool uModEnabled;

    public UModSwitchView(
        IUModService uModService,
        IViewManager viewManager,
        ILogger<UModSwitchView> logger)
    {
        this.uModService = uModService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
        this.UModEnabled = this.uModService.Enabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.uModService.Enabled = !this.uModService.Enabled;
        this.viewManager.ShowView<LauncherView>();
    }

    private void Wiki_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<UModBrowserView>(WikiLink);
    }

    private void Mods_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        this.viewManager.ShowView<UModBrowserView>(ModsLink);
    }
}
