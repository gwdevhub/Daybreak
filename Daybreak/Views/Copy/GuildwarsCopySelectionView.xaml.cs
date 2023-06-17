using Daybreak.Configuration.Options;
using Daybreak.Controls.Buttons;
using Daybreak.Models;
using Daybreak.Models.Onboarding;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Copy;
/// <summary>
/// Interaction logic for CopySelectionView.xaml
/// </summary>
public partial class GuildwarsCopySelectionView : UserControl
{
    private readonly IViewManager viewManager;
    private readonly IOptions<LauncherOptions> options;
    private readonly ILogger<GuildwarsCopySelectionView> logger;

    [GenerateDependencyProperty]
    private IEnumerable<string> existingPaths = default!;

    public GuildwarsCopySelectionView(
        IViewManager viewManager,
        IOptions<LauncherOptions> options,
        ILogger<GuildwarsCopySelectionView> logger)
    {
        this.viewManager = viewManager.ThrowIfNull();
        this.options = options.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
    }

    private void UserControl_Loaded(object _, RoutedEventArgs __)
    {
        var paths = this.options.Value?.GuildwarsPaths ?? new List<GuildwarsPath>();
        this.ExistingPaths = paths.Select(p => p.Path);
        if (paths.None())
        {
            this.viewManager.ShowView<LauncherOnboardingView>(LauncherOnboardingStage.NeedsExecutable);
        }
    }

    private void CopyButton_Clicked(object sender, EventArgs _)
    {
        if (sender is not HighlightButton highlightButton ||
            highlightButton.DataContext is not string path)
        {
            return;
        }

        this.viewManager.ShowView<GuildwarsCopyView>(path);
    }
}
