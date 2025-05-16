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
using System.Linq;
using System.Collections.Generic;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Navigation;
using System.Threading;
using System.Windows.Forms.VisualStyles;

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

    [GenerateDependencyProperty]
    private List<SynchronizationBuild> remoteBuildEntries = default!;
    [GenerateDependencyProperty]
    private List<SynchronizationBuild> localBuildEntries = default!;

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
        await Task.Factory.StartNew(async () =>
        {
            var profile = await this.graphClient.GetUserProfile<BuildsSynchronizationView>();
            if (profile.TryExtractSuccess(out var user) is false)
            {
                this.logger.LogError("Failed to get user info");
                return;
            }

            await this.Dispatcher.InvokeAsync(() => this.DisplayName = user?.DisplayName, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
            await this.PopulateBuilds();
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        await this.Dispatcher.InvokeAsync(() =>
        {

            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
    }

    private async Task PopulateBuilds()
    {
        var getBuildsResponse = await this.graphClient.RetrieveBuildsList();
        if (getBuildsResponse.TryExtractSuccess(out var remoteBuildFiles) is false ||
            remoteBuildFiles is null)
        {
            remoteBuildFiles = [];
        }

        var localBuildFiles = await this.buildTemplateManager.GetBuilds().ToListAsync();

        var remoteBuilds = remoteBuildFiles.Select(buildFile => new SynchronizationBuild { Name = buildFile.FileName, TemplateCode = buildFile.TemplateCode })
            .ToList();
        var localBuilds = localBuildFiles.Select(build => new SynchronizationBuild { Name = build.Name!, TemplateCode = this.buildTemplateManager.EncodeTemplate(build) })
            .ToList();

        var changedLocalBuilds = localBuilds.Where(
            localBuild => remoteBuilds.None(remoteBuild => localBuild.Name + localBuild.TemplateCode == remoteBuild.Name + remoteBuild.TemplateCode))
            .ToList();
        var changedRemoteBuilds = remoteBuilds.Where(
            remoteBuild => localBuilds.None(localBuild => localBuild.Name + localBuild.TemplateCode == remoteBuild.Name + remoteBuild.TemplateCode))
            .ToList();

        if (changedLocalBuilds.Count is 0 &&
            changedRemoteBuilds.Count is 0)
        {
            await this.Dispatcher.InvokeAsync(() => this.Synchronized = true, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
        }
        else
        {
            await this.Dispatcher.InvokeAsync(() => this.Synchronized = false, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
        }

        foreach(var build in changedLocalBuilds.Concat(changedRemoteBuilds))
        {
            build.Changed = true;
        }

        await this.Dispatcher.InvokeAsync(() =>
        {
            this.RemoteBuildEntries = remoteBuilds;
            this.LocalBuildEntries = localBuilds;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
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
        await Task.Factory.StartNew(async () =>
        {
            await this.graphClient.UploadBuild(build.Name!);
            await this.PopulateBuilds();
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
    }

    private async void DownloadButton_Clicked(object sender, EventArgs e)
    {
        if (this.SelectedRemoteBuild is not SynchronizationBuild build)
        {
            return;
        }

        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await Task.Factory.StartNew(async () =>
        {
            await this.graphClient.DownloadBuild(build.Name!);
            await this.PopulateBuilds();
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
    }

    private async void DownloadAllButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await Task.Factory.StartNew(async () =>
        {
            var result = await this.graphClient.DownloadBuilds();
            result.DoAny(
                onFailure: (failure) =>
                {
                    this.logger.LogError(failure, $"Failed to download builds");
                });
            await this.PopulateBuilds();
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
    }

    private async void UploadAllButton_Clicked(object sender, EventArgs e)
    {
        this.ButtonsEnabled = false;
        this.ShowLoading = true;
        await Task.Factory.StartNew(async () =>
        {
            var result = await this.graphClient.UploadBuilds();
            result.DoAny(
                onFailure: (failure) =>
                {
                    this.logger.LogError(failure, $"Failed to upload builds");
                });
            await this.PopulateBuilds();
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).Unwrap();
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
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
        await this.Dispatcher.InvokeAsync(() =>
        {
            this.ButtonsEnabled = true;
            this.ShowLoading = false;
        }, System.Windows.Threading.DispatcherPriority.Background, CancellationToken.None);
    }
}
