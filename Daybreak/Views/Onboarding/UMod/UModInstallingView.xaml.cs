﻿using Daybreak.Models.Progress;
using Daybreak.Services.Navigation;
using Daybreak.Services.UMod;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.UMod;
/// <summary>
/// Interaction logic for UModInstallerView.xaml
/// </summary>
public partial class UModInstallingView : UserControl
{
    private readonly ILogger<UModInstallingView> logger;
    private readonly IViewManager viewManager;
    private readonly IUModService uModService;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    public UModInstallingView(
        IUModService uModService,
        ILogger<UModInstallingView> logger,
        IViewManager viewManager)
    {
        this.uModService = uModService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<UModInstallationStatus>();
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (installationStatus?.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = installationStatus?.CurrentStep.Description;
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        var installationStatus = new UModInstallationStatus();
        installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
        await this.uModService.SetupUMod(installationStatus);
        installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
        this.ContinueButtonEnabled = true;
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<UModMainView>();
    }
}
