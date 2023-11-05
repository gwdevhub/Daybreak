﻿using Daybreak.Models.Progress;
using Daybreak.Services.DirectSong;
using Daybreak.Services.Navigation;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Extensions;

namespace Daybreak.Views.Onboarding.DirectSong;
/// <summary>
/// Interaction logic for DirectSongOnboardingView.xaml
/// </summary>
public partial class DirectSongInstallationView : UserControl
{
    private readonly ILogger<DirectSongInstallationView> logger;
    private readonly IViewManager viewManager;
    private readonly IDirectSongService directSongService;

    [GenerateDependencyProperty(InitialValue = "")]
    private string description = string.Empty;
    [GenerateDependencyProperty]
    private double progressValue;
    [GenerateDependencyProperty]
    private bool continueButtonEnabled;
    [GenerateDependencyProperty(InitialValue = false)]
    private bool progressVisible;

    private DirectSongInstallationStatus? installationStatus;

    public DirectSongInstallationView(
        IDirectSongService directSongService,
        ILogger<DirectSongInstallationView> logger,
        IViewManager viewManager)
    {
        this.directSongService = directSongService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.InitializeComponent();
    }

    private void DownloadStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var installationStatus = sender?.As<DirectSongInstallationStatus>();
        this.Dispatcher.Invoke(() =>
        {
            this.ProgressVisible = false;
            if (installationStatus?.CurrentStep is DownloadStatus.DownloadProgressStep downloadUpdateStep)
            {
                this.ProgressValue = downloadUpdateStep.Progress * 100;
                this.ProgressVisible = true;
            }
            else if (installationStatus?.CurrentStep is DirectSongInstallationStatus.DirectSongInstallationProgressStep progressStep)
            {
                this.ProgressValue = progressStep.Progress * 100;
                this.ProgressVisible = true;
            }

            this.Description = installationStatus?.CurrentStep.Description;
        });
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.installationStatus = this.directSongService.CachedInstallationStatus;
        if (this.installationStatus is null)
        {
            this.installationStatus = new DirectSongInstallationStatus();
            this.installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
            await this.directSongService.SetupDirectSong(this.installationStatus, CancellationToken.None);
            this.installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
            this.ContinueButtonEnabled = true;
            return;
        }
        else
        {
            this.installationStatus.PropertyChanged += this.DownloadStatus_PropertyChanged;
            await (this.directSongService.InstallationTask ?? throw new InvalidOperationException("Installation task is null but the installation status cache exists"));
            this.installationStatus.PropertyChanged -= this.DownloadStatus_PropertyChanged;
            this.ContinueButtonEnabled = true;
        }
    }

    private void OpaqueButton_Clicked(object sender, System.EventArgs e)
    {
        this.viewManager.ShowView<DirectSongSwitchView>();
    }
}
