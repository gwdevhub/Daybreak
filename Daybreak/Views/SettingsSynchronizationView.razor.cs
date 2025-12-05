using Daybreak.Services.Graph;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Core.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class SettingsSynchronizationViewModel(
    INotificationService notificationService,
    IGraphClient graphClient,
    IOptionsSynchronizationService optionsSynchronizationService)
    : ViewModelBase<SettingsSynchronizationViewModel, SettingsSynchronizationView>
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IGraphClient graphClient = graphClient.ThrowIfNull();
    private readonly IOptionsSynchronizationService optionsSynchronizationService = optionsSynchronizationService.ThrowIfNull();

    private Dictionary<string, JObject>? remoteOptions;
    private Dictionary<string, JObject>? localOptions;

    public bool Loading { get; set; }

    public SideBySideDiffModel? SideBySideDiff { get; set; }

    public override ValueTask ParametersSet(SettingsSynchronizationView view, CancellationToken cancellationToken)
    {
        this.Loading = true;
        this.AsyncLoadOptions();
        return base.ParametersSet(view, cancellationToken);
    }

    public async void SaveChanges()
    {
        this.Loading = true;
        await this.optionsSynchronizationService.BackupOptions(CancellationToken.None);
        this.Loading = false;
    }

    public async void UndoChanges()
    {
        this.Loading = true;
        await this.optionsSynchronizationService.RestoreOptions(CancellationToken.None);
        this.Loading = false;
    }

    public void CalculateDiff()
    {
        var localOptionsString = this.localOptions is null
            ? "{}"
            : JsonConvert.SerializeObject(this.localOptions, Formatting.Indented);

        var remoteOptionsString = this.remoteOptions is null
            ? "{}"
            : JsonConvert.SerializeObject(this.remoteOptions, Formatting.Indented);

        var diffBuilder = new SideBySideDiffBuilder(new Differ());
        this.SideBySideDiff = diffBuilder.BuildDiffModel(localOptionsString, remoteOptionsString);
    }

    private async void AsyncLoadOptions()
    {
        this.remoteOptions = await this.optionsSynchronizationService.GetRemoteOptions(CancellationToken.None);
        if (this.remoteOptions is null)
        {
            await this.graphClient.PerformAuthorizationFlow(CancellationToken.None);
            this.remoteOptions = await this.optionsSynchronizationService.GetRemoteOptions(CancellationToken.None);
            if (this.remoteOptions is null)
            {
                this.notificationService.NotifyError(
                    title: "Synchronization failure",
                    description: "Failed to fetch remote options. Check logs for details");
                return;
            }
        }

        this.localOptions = await this.optionsSynchronizationService.GetLocalOptions(CancellationToken.None);
        this.Loading = false;
        this.CalculateDiff();
        await this.RefreshViewAsync();
    }
}
