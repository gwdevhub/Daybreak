using Daybreak.Services.Guildwars.Models;
using Daybreak.Shared.Models.Progress;
using Daybreak.Shared.Services.Guildwars;
using Daybreak.Shared.Services.Menu;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Installation;

/// <summary>
/// Interaction logic for DownloadView.xaml
/// </summary>
public partial class GuildWarsDownloadView : UserControl
{
    private readonly IMenuService menuService;
    private readonly ILogger<GuildWarsDownloadView> logger;
    //private readonly IViewManager viewManager;
    private readonly IGuildWarsInstaller guildwarsInstaller;
    
    private GuildwarsInstallationStatus? installationStatus;
    private CancellationTokenSource? cancellationTokenSource;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty(InitialValue = true)]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty]
    private bool progressVisible;

    public GuildWarsDownloadView(
        IMenuService menuService,
        IGuildWarsInstaller guildwarsInstaller,
        //IViewManager viewManager,
        ILogger<GuildWarsDownloadView> logger)
    {
        this.menuService = menuService.ThrowIfNull();
        this.guildwarsInstaller = guildwarsInstaller.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        //this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var newDescription = this.installationStatus?.CurrentStep.Description ?? string.Empty;
        var newProgressValue = (int)(this.installationStatus?.CurrentStep.As<DownloadStatus.DownloadProgressStep>()?.Progress * 100 ?? 0);
        // Skip dispatcher invokation for no visible changes
        if (this.description == newDescription &&
            this.progressValue == newProgressValue)
        {
            return;
        }

        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (this.installationStatus?.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = newProgressValue;
                this.ProgressVisible = true;
            }

            if (this.installationStatus?.CurrentStep is GuildwarsInstallationStatus.GuildwarsInstallationStep step &&
                step.Final)
            {
                this.ContinueButtonEnabled = true;
            }
            else
            {
                this.ContinueButtonEnabled = false;
            }

            this.Description = newDescription;
        });
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        //this.viewManager.ShowView<LauncherView>();
    }

    private void DownloadView_Unloaded(object sender, RoutedEventArgs e)
    {
        this.cancellationTokenSource?.Cancel();
        this.cancellationTokenSource?.Dispose();
    }

    private void GuildWarsDownloadView_DataContextChanged(object _, DependencyPropertyChangedEventArgs __)
    {
        if (this.DataContext is not GuildWarsDownloadContext context ||
            context.GuildwarsInstallationStatus is null ||
            context.CancellationTokenSource is null)
        {
            return;
        }

        if (this.cancellationTokenSource is not null)
        {
            this.cancellationTokenSource.Cancel();
            this.cancellationTokenSource.Dispose();
            this.cancellationTokenSource = null;
        }

        if (this.installationStatus is not null)
        {
            this.installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        }

        this.ContinueButtonEnabled = false;
        this.menuService.CloseMenu();
        this.installationStatus = context.GuildwarsInstallationStatus;
        this.cancellationTokenSource = context.CancellationTokenSource;
        this.installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
    }
}
