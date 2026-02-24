using Daybreak.Services.Graph;
using Daybreak.Shared.Extensions;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.Notifications;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Core.Extensions;
using System.Drawing;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class BuildsSynchronizationViewModel
    : ViewModelBase<BuildsSynchronizationViewModel, BuildsSynchronizationView>
{
    private readonly DotNetObjectReference<BuildsSynchronizationViewModel> dotNetObjectReference;
    private readonly INotificationService notificationService;
    private readonly IGraphClient graphClient;
    private readonly IBuildTemplateManager buildTemplateManager;
    private readonly IJSRuntime jsRuntime;

    private readonly Dictionary<string, IBuildEntry> localBuildEntries = [];
    private readonly Dictionary<string, IBuildEntry> remoteBuildEntries = [];

    public BuildsSynchronizationViewModel(
        INotificationService notificationService,
        IGraphClient graphClient,
        IBuildTemplateManager buildTemplateManager,
        IJSRuntime jsRuntime)
    {
        this.dotNetObjectReference = DotNetObjectReference.Create(this);
        this.notificationService = notificationService.ThrowIfNull();
        this.graphClient = graphClient.ThrowIfNull();
        this.buildTemplateManager = buildTemplateManager.ThrowIfNull();
        this.jsRuntime = jsRuntime.ThrowIfNull();
    }

    public bool Loading { get; set; }
    public List<BuildDiffItem> LocalBuilds { get; set; } = [];
    public List<BuildDiffItem> RemoteBuilds { get; set; } = [];

    public BuildDiffItem? HoveredEntry { get; private set; }
    public Point? SnippetPosition { get; private set; }
    public bool ShowSnippet { get; private set; }

    public override ValueTask ParametersSet(BuildsSynchronizationView view, CancellationToken cancellationToken)
    {
        this.Loading = true;
        this.AsyncLoadBuilds();
        return base.ParametersSet(view, cancellationToken);
    }

    public async void UploadAllBuilds()
    {
        this.Loading = true;
        await this.RefreshViewAsync();

        var success = await this.graphClient.UploadBuilds();
        if (success)
        {
            this.notificationService.NotifyInformation(
                title: "Builds uploaded",
                description: "All local builds have been uploaded to remote storage");
            await this.ReloadBuildsAndCalculateDiff();
        }
        else
        {
            this.notificationService.NotifyError(
                title: "Upload failed",
                description: "Failed to upload builds. Check logs for details");
        }

        this.Loading = false;
        await this.RefreshViewAsync();
    }

    public async void DownloadAllBuilds()
    {
        this.Loading = true;
        await this.RefreshViewAsync();

        var success = await this.graphClient.DownloadBuilds();
        if (success)
        {
            this.notificationService.NotifyInformation(
                title: "Builds downloaded",
                description: "All remote builds have been downloaded to local storage");
            await this.ReloadBuildsAndCalculateDiff();
        }
        else
        {
            this.notificationService.NotifyError(
                title: "Download failed",
                description: "Failed to download builds. Check logs for details");
        }

        this.Loading = false;
        await this.RefreshViewAsync();
    }

    public async void UploadSingleBuild(BuildDiffItem buildDiffItem)
    {
        if (buildDiffItem.BuildEntry is null ||
            string.IsNullOrEmpty(buildDiffItem.Name) ||
            buildDiffItem.Status == BuildDiffStatus.InSync)
        {
            return;
        }

        this.CloseSnippet();
        this.Loading = true;
        await this.RefreshViewAsync();

        var success = await this.graphClient.UploadBuild(buildDiffItem.Name);
        if (success)
        {
            this.notificationService.NotifyInformation(
                title: "Build uploaded",
                description: $"'{buildDiffItem.Name}' has been uploaded to remote storage");
            await this.ReloadBuildsAndCalculateDiff();
        }
        else
        {
            this.notificationService.NotifyError(
                title: "Upload failed",
                description: $"Failed to upload '{buildDiffItem.Name}'. Check logs for details");
        }

        this.Loading = false;
        await this.RefreshViewAsync();
    }

    public async void DownloadSingleBuild(BuildDiffItem buildDiffItem)
    {
        if (buildDiffItem.BuildEntry is null ||
            string.IsNullOrEmpty(buildDiffItem.Name) ||
            buildDiffItem.Status == BuildDiffStatus.InSync)
        {
            return;
        }

        this.CloseSnippet();
        this.Loading = true;
        await this.RefreshViewAsync();

        var success = await this.graphClient.DownloadBuild(buildDiffItem.Name);
        if (success)
        {
            this.notificationService.NotifyInformation(
                title: "Build downloaded",
                description: $"'{buildDiffItem.Name}' has been downloaded to local storage");
            await this.ReloadBuildsAndCalculateDiff();
        }
        else
        {
            this.notificationService.NotifyError(
                title: "Download failed",
                description: $"Failed to download '{buildDiffItem.Name}'. Check logs for details");
        }

        this.Loading = false;
        await this.RefreshViewAsync();
    }

    private async void AsyncLoadBuilds()
    {
        var authSuccess = await this.graphClient.PerformAuthorizationFlow(CancellationToken.None);
        if (!authSuccess)
        {
            this.notificationService.NotifyError(
                title: "Authentication failed",
                description: "Failed to authenticate with Microsoft Graph. Check logs for details");
            this.Loading = false;
            await this.RefreshViewAsync();
            return;
        }

        await this.ReloadBuildsAndCalculateDiff();

        this.Loading = false;
        await this.RefreshViewAsync();
    }

    private async Task ReloadBuildsAndCalculateDiff()
    {
        this.localBuildEntries.Clear();
        this.remoteBuildEntries.Clear();

        await foreach (var build in this.buildTemplateManager.GetBuilds())
        {
            var name = build.Name ?? "Unnamed";
            this.localBuildEntries[name] = build;
        }

        var remoteBuilds = await this.graphClient.RetrieveBuildsList();
        var remoteBuildsList = remoteBuilds?.ToList() ?? [];

        foreach (var buildFile in remoteBuildsList)
        {
            if (buildFile.FileName is null || buildFile.TemplateCode is null)
            {
                continue;
            }

            if (this.buildTemplateManager.TryDecodeTemplate(buildFile.TemplateCode, out var build))
            {
                build.Name = buildFile.FileName;
                build.SourceUrl = buildFile.SourceUrl;
                build.Metadata = buildFile.Metadata;
                this.remoteBuildEntries[buildFile.FileName] = build;
            }
        }

        this.CalculateDiff();
    }

    private void CalculateDiff()
    {
        var allNames = this.localBuildEntries.Keys
            .Union(this.remoteBuildEntries.Keys)
            .OrderBy(n => n)
            .ToList();

        this.LocalBuilds.Clear();
        this.RemoteBuilds.Clear();

        foreach (var name in allNames)
        {
            var existsLocally = this.localBuildEntries.TryGetValue(name, out var localBuild);
            var existsRemotely = this.remoteBuildEntries.TryGetValue(name, out var remoteBuild);

            var localCode = existsLocally ? this.buildTemplateManager.EncodeTemplate(localBuild!) : string.Empty;
            var remoteCode = existsRemotely ? this.buildTemplateManager.EncodeTemplate(remoteBuild!) : string.Empty;

            if (existsLocally && existsRemotely)
            {
                var codesMatch = localCode == remoteCode;
                var status = codesMatch ? BuildDiffStatus.InSync : BuildDiffStatus.Modified;

                this.LocalBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = localCode,
                    Status = status,
                    BuildEntry = localBuild
                });

                this.RemoteBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = remoteCode,
                    Status = status,
                    BuildEntry = remoteBuild
                });
            }
            else if (existsLocally)
            {
                this.LocalBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = localCode,
                    Status = BuildDiffStatus.LocalOnly,
                    BuildEntry = localBuild
                });

                this.RemoteBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = string.Empty,
                    Status = BuildDiffStatus.Placeholder,
                    BuildEntry = null
                });
            }
            else if (existsRemotely)
            {
                this.LocalBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = string.Empty,
                    Status = BuildDiffStatus.Placeholder,
                    BuildEntry = null
                });

                this.RemoteBuilds.Add(new BuildDiffItem
                {
                    Name = name,
                    TemplateCode = remoteCode,
                    Status = BuildDiffStatus.RemoteOnly,
                    BuildEntry = remoteBuild
                });
            }
        }
    }

    public async void OpenSnippet(BuildDiffItem buildDiffItem, MouseEventArgs e)
    {
        if (buildDiffItem.BuildEntry is null)
        {
            return;
        }

        this.ShowSnippet = false;
        this.HoveredEntry = buildDiffItem;
        this.SnippetPosition = new Point((int)e.ClientX, (int)e.ClientY);
        await this.jsRuntime.HoverDelayStart(this.dotNetObjectReference, nameof(this.HoverComplete));
    }

    public async void CloseSnippet()
    {
        this.HoveredEntry = default;
        this.ShowSnippet = false;
        await this.jsRuntime.HoverDelayStop();
    }

    public void MouseMoveBuildEntry(MouseEventArgs e)
    {
        if (!this.ShowSnippet)
        {
            this.SnippetPosition = new Point((int)e.ClientX, (int)e.ClientY);
        }
    }

    [JSInvokable]
    public void HoverComplete()
    {
        this.ShowSnippet = true;
        this.RefreshView();
    }
}

public sealed class LocalBuildItem
{
    public string Name { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
}

public sealed class BuildDiffItem
{
    public string Name { get; set; } = string.Empty;
    public string TemplateCode { get; set; } = string.Empty;
    public BuildDiffStatus Status { get; set; }
    public IBuildEntry? BuildEntry { get; set; }
}

public enum BuildDiffStatus
{
    InSync,
    LocalOnly,
    RemoteOnly,
    Modified,
    Placeholder
}
