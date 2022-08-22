using Daybreak.Services.Graph;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Controls;
using Daybreak.Services.ViewManagement;
using System.Collections.ObjectModel;
using Daybreak.Services.Graph.Models;
using System.Extensions;
using System.Threading.Tasks;
using System;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for BuildsSynchronizationView.xaml
/// </summary>
public partial class BuildsSynchronizationView : UserControl
{
    private readonly IGraphClient graphClient;
    private readonly IViewManager viewManager;
    private readonly ILogger<BuildsSynchronizationView> logger;

    public ObservableCollection<BuildFile> BuildEntries { get; } = new();

    [GenerateDependencyProperty(InitialValue = true)]
    private bool buttonsEnabled;
    [GenerateDependencyProperty]
    private string displayName;
    [GenerateDependencyProperty]
    private string lastUploadDate;

    public BuildsSynchronizationView(
        IGraphClient graphClient,
        IViewManager viewManager,
        ILogger<BuildsSynchronizationView> logger)
    {
        this.graphClient = graphClient.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.ButtonsEnabled = false;
        var profile = await this.graphClient.GetUserProfile<BuildsSynchronizationView>();
        if (profile.TryExtractSuccess(out var user) is false)
        {
            this.logger.LogError("Failed to get user info");
            return;
        }

        this.DisplayName = user.DisplayName;
        await this.PopulateLastUpdateTime();
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
    }

    private async Task PopulateLastUpdateTime()
    {
        var maybeDateTime = await this.graphClient.GetLastUpdateTime();
        if (maybeDateTime.TryExtractSuccess(out var dateTime))
        {
            this.LastUploadDate = dateTime.ToString("G");
        }
        else
        {
            this.LastUploadDate = "Never";
        }
    }

    private async Task PopulateBuilds()
    {
        var maybeBuilds = await this.graphClient.RetrieveBuildsList();
        if (maybeBuilds.TryExtractSuccess(out var builds))
        {
            this.BuildEntries.ClearAnd().AddRange(builds);
        }
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<BuildsListView>();
    }

    private async void UploadButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        var result = await this.graphClient.UploadBuilds();
        result.DoAny(
            onFailure: (failure) =>
            {
                this.logger.LogError(failure, $"Failed to upload builds");
            });

        await this.PopulateLastUpdateTime();
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        var result = await this.graphClient.DownloadBuilds();
        result.DoAny(
            onFailure: (failure) =>
            {
                this.logger.LogError(failure, $"Failed to download builds");
            });

        await this.PopulateLastUpdateTime();
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
    }
}
