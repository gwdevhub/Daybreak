using Daybreak.Controls.Buttons;
using Daybreak.Shared.Services.ExecutableManagement;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Copy;
/// <summary>
/// Interaction logic for CopySelectionView.xaml
/// </summary>
public partial class GuildwarsCopySelectionView : UserControl
{
    //private readonly IViewManager viewManager;
    private readonly IGuildWarsExecutableManager guildWarsExecutableManager;
    private readonly ILogger<GuildwarsCopySelectionView> logger;

    [GenerateDependencyProperty]
    private IEnumerable<string> existingPaths = default!;

    public GuildwarsCopySelectionView(
        //IViewManager viewManager,
        IGuildWarsExecutableManager guildWarsExecutableManager,
        ILogger<GuildwarsCopySelectionView> logger)
    {
        //this.viewManager = viewManager.ThrowIfNull();
        this.guildWarsExecutableManager = guildWarsExecutableManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object _, RoutedEventArgs __)
    {
        var executables = this.guildWarsExecutableManager.GetExecutableList();
        this.ExistingPaths = executables;
        if (executables.None())
        {
            //this.viewManager.ShowView<LauncherOnboardingView>(LauncherOnboardingStage.NeedsExecutable);
        }
    }

    private void CopyButton_Clicked(object sender, EventArgs _)
    {
        if (sender is not HighlightButton highlightButton ||
            highlightButton.DataContext is not string path)
        {
            return;
        }

        //this.viewManager.ShowView<GuildwarsCopyView>(path);
    }
}
