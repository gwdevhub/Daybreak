using Daybreak.Services.DirectSong;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows.Controls;

namespace Daybreak.Views.Onboarding.DirectSong;
/// <summary>
/// Interaction logic for DirectSongOnboardingEntryView.xaml
/// </summary>
public partial class DirectSongOnboardingEntryView : UserControl
{
    private readonly IDirectSongService directSongService;
    private readonly IViewManager viewManager;
    private readonly ILogger<DirectSongOnboardingEntryView> logger;

    public DirectSongOnboardingEntryView(
        IDirectSongService directSongService,
        IViewManager viewManager,
        ILogger<DirectSongOnboardingEntryView> logger)
    {
        this.directSongService = directSongService.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();

        this.InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        if (this.directSongService.IsInstalled)
        {
            this.viewManager.ShowView<DirectSongSwitchView>();
        }
        else if (this.directSongService.CachedInstallationStatus is not null)
        {
            this.viewManager.ShowView<DirectSongInstallationView>();
        }
        else
        {
            this.viewManager.ShowView<DirectSongInstallationChoiceView>();
        }
    }
}
