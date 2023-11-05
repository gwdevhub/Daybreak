using Daybreak.Services.DirectSong;
using Daybreak.Services.Navigation;
using Daybreak.Services.Toolbox;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.DirectSong;
/// <summary>
/// Interaction logic for DirectSongInstallationChoiceView.xaml
/// </summary>
public partial class DirectSongInstallationChoiceView : UserControl
{
    private readonly IDirectSongService directSongService;
    private readonly IViewManager viewManager;
    private readonly ILogger<DirectSongInstallationChoiceView> logger;

    public DirectSongInstallationChoiceView(
        IDirectSongService directSongService,
        IViewManager viewManager,
        ILogger<DirectSongInstallationChoiceView> logger)
    {
        this.directSongService = directSongService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void OpaqueButtonNo_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<LauncherView>();
    }

    private void OpaqueButtonYes_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<DirectSongInstallationView>();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.directSongService.IsInstalled)
        {
            this.viewManager.ShowView<DirectSongOnboardingEntryView>();
        }
    }
}
