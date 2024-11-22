using Daybreak.Services.DirectSong;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DirectSong;
/// <summary>
/// Interaction logic for DirectSongSwitchView.xaml
/// </summary>
public partial class DirectSongSwitchView : UserControl
{
    private readonly IDirectSongService directSongService;
    private readonly IViewManager viewManager;
    private readonly ILogger<DirectSongSwitchView> logger;

    [GenerateDependencyProperty]
    private bool directSongEnabled;

    public DirectSongSwitchView(
        IDirectSongService directSongService,
        IViewManager viewManager,
        ILogger<DirectSongSwitchView> logger)
    {
        this.directSongService = directSongService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
        this.DirectSongEnabled = this.directSongService.IsEnabled;
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.directSongService.IsEnabled = !this.directSongService.IsEnabled;
        this.viewManager.ShowView<LauncherView>();
    }

    private void Wiki_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
    }
}
