using Daybreak.Services.Graph;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Windows;
using System.Windows.Extensions;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Daybreak.Services.Graph.Models;
using System.Extensions;
using System.Threading.Tasks;
using System;
using Daybreak.Services.BuildTemplates;
using System.Linq;
using Daybreak.Models;
using System.Collections.Generic;
using Daybreak.Services.Navigation;

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

    public ObservableCollection<SynchronizationBuild> RemoteBuildEntries { get; } = [];
    public ObservableCollection<SynchronizationBuild> LocalBuildEntries { get; } = [];

    [GenerateDependencyProperty(InitialValue = true)]
    private bool buttonsEnabled;
    [GenerateDependencyProperty]
    private string displayName = string.Empty;
    [GenerateDependencyProperty]
    private string lastUploadDate = string.Empty;
    [GenerateDependencyProperty]
    private SynchronizationBuild selectedRemoteBuild = default!;
    [GenerateDependencyProperty]
    private SynchronizationBuild selectedLocalBuild = default!;
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

        this.DisplayName = user?.DisplayName;
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async Task PopulateBuilds()
    {
        var getBuildsResponse = await this.graphClient.RetrieveBuildsList();
        if (getBuildsResponse.TryExtractSuccess(out var remoteBuildFiles) is false)
        {
            remoteBuildFiles = new List<BuildFile>();
        }

        var localBuildFiles = await this.buildTemplateManager.GetBuilds().ToListAsync();

        var remoteBuilds = remoteBuildFiles!.Select(buildFile => new SynchronizationBuild { Name = buildFile.FileName!, TemplateCode = buildFile.TemplateCode! })
            .ToList();
        var localBuilds = localBuildFiles.Select(build => new SynchronizationBuild { Name = build.Name!, TemplateCode = this.buildTemplateManager.EncodeTemplate(build.Build!) })
            .ToList();

        var changedLocalBuilds = localBuilds.Where(
            localBuild => remoteBuilds.None(remoteBuild => localBuild.Name + localBuild.TemplateCode == remoteBuild.Name + remoteBuild.TemplateCode))
            .ToList();
        var changedRemoteBuilds = remoteBuilds.Where(
            remoteBuild => localBuilds.None(localBuild => localBuild.Name + localBuild.TemplateCode == remoteBuild.Name + remoteBuild.TemplateCode))
            .ToList();

        if (localBuilds.Any() &&
            changedLocalBuilds.None() &&
            changedRemoteBuilds.None())
        {
            this.Synchronized = true;
        }
        else
        {
            this.Synchronized = false;
        }

        foreach(var build in changedLocalBuilds.Concat(changedRemoteBuilds))
        {
            build.Changed = true;
        }

        this.RemoteBuildEntries.ClearAnd().AddRange(remoteBuilds);
        this.LocalBuildEntries.ClearAnd().AddRange(localBuilds);
    }

    private void BackButton_Clicked(object sender, EventArgs e)
    {
        this.viewManager.ShowView<BuildsListView>();
    }

    private async void UploadButton_Clicked(object sender, EventArgs e)
    {
        // TODO: Handle failures to upload
        if (this.SelectedLocalBuild is not SynchronizationBuild build)
        {
            return;
        }

        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await this.graphClient.UploadBuild(build.Name!);
        await this.PopulateBuilds();
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        if (this.SelectedRemoteBuild is not SynchronizationBuild build)
        {
            return;
        }

        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await this.graphClient.DownloadBuild(build.Name!);
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

    private async void LogOutButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        var result = await this.graphClient.LogOut();
        result.Do(
            onSuccess: logOutSuccess =>
            {
                if (logOutSuccess)
                {
                    this.viewManager.ShowView<BuildsListView>();
                }
            },
            onFailure: failure =>
            {
                this.logger.LogError(failure, "Failed to log out");
            });
        this.ButtonsEnabled = true;
        this.ShowLoading = false;
    }
}
