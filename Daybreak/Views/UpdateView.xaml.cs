using Daybreak.Models.Progress;
using Daybreak.Services.Navigation;
using Daybreak.Services.Updater;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;
using Version = Daybreak.Models.Versioning.Version;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for UpdateView.xaml
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Fields used by source generator for DependencyProperty")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used by source generators")]
public partial class UpdateView : UserControl
{
    private readonly ILogger<UpdateView> logger;
    private readonly IViewManager viewManager;
    private readonly IApplicationUpdater applicationUpdater;
    private readonly UpdateStatus updateStatus = new();

    private bool success = false;
    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty]
    private TimeSpan eta;
    [GenerateDependencyProperty]
    private bool etaVisible;

    public UpdateView(
        IApplicationUpdater applicationUpdater,
        ILogger<UpdateView> logger,
        IViewManager viewManager)
    {
        this.applicationUpdater = applicationUpdater.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.updateStatus.PropertyChanged += this.UpdateStatus_PropertyChanged!;
        this.InitializeComponent();
    }

    private void UpdateStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        this.Dispatcher.Invoke(() =>
        {
            if (this.updateStatus.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.Eta = downloadUpdateStep.ETA ?? TimeSpan.Zero;
                this.EtaVisible = downloadUpdateStep.ETA is not null;
            }

            this.Description = this.updateStatus.CurrentStep.Description;
        });
    }

    private async void UpdateView_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is not Version version)
        {
            this.viewManager.ShowView<LauncherView>();
            throw new InvalidOperationException("No version specified for download");
        }

        this.logger.LogInformation("Starting update procedure");
        var success = await this.applicationUpdater.DownloadUpdate(version, this.updateStatus).ConfigureAwait(true);
        if (success is false)
        {
            this.logger.LogError("Update procedure failed");
        }
        else
        {
            this.success= true;
            this.logger.LogInformation("Downloaded update");
        }

        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        if (this.success)
        {
            this.applicationUpdater.FinalizeUpdate();
            Application.Current.Shutdown();
        }
        else
        {
            this.viewManager.ShowView<LauncherView>();
        }
    }
}
