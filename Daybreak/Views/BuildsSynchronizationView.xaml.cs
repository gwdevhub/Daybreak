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
using Daybreak.Services.BuildTemplates;
using System.Linq;
using Daybreak.Models;
using System.Collections.Generic;

namespace Daybreak.Views;

/// <summary>
/// Interaction logic for BuildsSynchronizationView.xaml
/// </summary>
public partial class BuildsSynchronizationView : UserControl
{
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IGraphClient graphClient;
    private readonly IViewManager viewManager;
    private readonly ILogger<BuildsSynchronizationView> logger;

    public ObservableCollection<BuildFile> RemoteBuildEntries { get; } = new();
    public ObservableCollection<BuildWithTemplateCode> LocalBuildEntries { get; } = new();

    [GenerateDependencyProperty(InitialValue = true)]
    private bool buttonsEnabled;
    [GenerateDependencyProperty]
    private string displayName;
    [GenerateDependencyProperty]
    private string lastUploadDate;
    [GenerateDependencyProperty]
    private BuildFile selectedRemoteBuild;
    [GenerateDependencyProperty]
    private BuildWithTemplateCode selectedLocalBuild;
    [GenerateDependencyProperty]
    private bool showLoading;
    [GenerateDependencyProperty]
    private bool synchronized;

    public BuildsSynchronizationView(
        IBuildTemplateManager buildTemplateManager,
        IGraphClient graphClient,
        IViewManager viewManager,
        ILogger<BuildsSynchronizationView> logger)
    {
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.graphClient = graphClient.ThrowIfNull();
        this.viewManager = viewManager.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
        this.InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        var profile = await this.graphClient.GetUserProfile<BuildsSynchronizationView>();
        if (profile.TryExtractSuccess(out var user) is false)
        {
            this.logger.LogError("Failed to get user info");
            return;
        }

        this.DisplayName = user.DisplayName;
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async Task PopulateBuilds()
    {
        var getBuildsResponse = await this.graphClient.RetrieveBuildsList();
        if (getBuildsResponse.TryExtractSuccess(out var builds) is false)
        {
            builds = new List<BuildFile>();
        }

        this.RemoteBuildEntries.ClearAnd().AddRange(builds);
        var localBuilds = await this.buildTemplateManager.GetBuilds().ToListAsync();
        this.LocalBuildEntries.ClearAnd().AddRange(localBuilds.Select(build => new BuildWithTemplateCode { Build = build, TemplateCode = this.buildTemplateManager.EncodeTemplate(build.Build) }));

        if (this.LocalBuildEntries.Count == this.RemoteBuildEntries.Count &&
            this.LocalBuildEntries.Select(b => b.Build.Name).Except(this.RemoteBuildEntries.Select(b => b.FileName)).None() &&
            this.LocalBuildEntries.Select(b => b.TemplateCode).Except(this.RemoteBuildEntries.Select(b => b.TemplateCode)).None())
        {
            this.Synchronized = true;
        }
        else
        {
            this.Synchronized = false;
        }
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<BuildsListView>();
    }

    private async void UploadButton_Clicked(object sender, EventArgs e)
    {
        // TODO: Handle failures to upload
        if (this.SelectedLocalBuild is not BuildWithTemplateCode buildWithTemplateCode)
        {
            return;
        }

        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await this.graphClient.UploadBuild(buildWithTemplateCode.Build.Name);
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        if (this.SelectedRemoteBuild is not BuildFile buildFile)
        {
            return;
        }

        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await this.graphClient.DownloadBuild(buildFile.FileName);
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async void DownloadAllButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        var result = await this.graphClient.DownloadBuilds().ConfigureAwait(true);
        result.DoAny(
            onFailure: (failure) =>
            {
                this.logger.LogError(failure, $"Failed to download builds");
            });

        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async void UploadAllButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        var result = await this.graphClient.UploadBuilds().ConfigureAwait(true);
        result.DoAny(
            onFailure: (failure) =>
            {
                this.logger.LogError(failure, $"Failed to upload builds");
            });

        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }
}
