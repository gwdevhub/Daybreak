﻿using Daybreak.Shared.Services.Navigation;
using Daybreak.Shared.Services.Updater;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Extensions;
using System.Windows.Controls;
using System.Windows.Extensions;
using Version = Daybreak.Shared.Models.Versioning.Version;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for VersionManagementView.xaml
/// </summary>
public partial class VersionManagementView : UserControl
{
    private const string VersionPlaceholder = "{VERSION}";
    private const string ReleaseURL = $"https://github.com/AlexMacocian/Daybreak/releases/tag/{VersionPlaceholder}";

    private readonly IApplicationUpdater applicationUpdater;
    private readonly IViewManager viewManager;

    [GenerateDependencyProperty]
    private Version currentVersion;

    [GenerateDependencyProperty]
    private bool loading;

    public ObservableCollection<Version> Versions { get; } = [];

    public VersionManagementView(
        IApplicationUpdater applicationUpdater,
        IViewManager viewManager)
    {
        this.applicationUpdater = applicationUpdater.ThrowIfNull(nameof(applicationUpdater));
        this.viewManager = viewManager.ThrowIfNull(nameof(viewManager));
        this.InitializeComponent();
        this.currentVersion = this.applicationUpdater.CurrentVersion;
        this.CurrentVersion = this.applicationUpdater.CurrentVersion;
        this.LoadVersionList();
    }

    private async void LoadVersionList()
    {
        this.Loading = true;
        this.Versions.ClearAnd().AddRange((await this.applicationUpdater.GetVersions()).Reverse());
        this.Loading = false;
    }

    private void CurrentVersion_Clicked(object sender, EventArgs e)
    {
        Process.Start("explorer.exe", ReleaseURL.Replace(VersionPlaceholder, this.currentVersion.ToString()));
    }

    private void DesiredVersion_Clicked(object sender, EventArgs e)
    {
        if (sender is not UserControl userControl)
        {
            return;
        }

        if (userControl.DataContext is not Version desiredVersion)
        {
            return;
        }

        Process.Start("explorer.exe", ReleaseURL.Replace(VersionPlaceholder, desiredVersion.ToString()));
    }

    private void DesiredVersion_DownloadButton_Clicked(object sender, EventArgs e)
    {
        if (sender is not UserControl userControl)
        {
            return;
        }

        if (userControl.DataContext is not Version desiredVersion)
        {
            return;
        }

        this.viewManager.ShowView<UpdateConfirmationView>(desiredVersion);
    }
}
