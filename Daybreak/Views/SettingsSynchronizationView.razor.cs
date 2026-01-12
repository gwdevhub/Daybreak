using Daybreak.Services.Graph;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Options;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using System.Core.Extensions;
using System.Text.Json;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class SettingsSynchronizationViewModel(
    INotificationService notificationService,
    IGraphClient graphClient,
    IOptionsSynchronizationService optionsSynchronizationService)
    : ViewModelBase<SettingsSynchronizationViewModel, SettingsSynchronizationView>
{
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };

    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IGraphClient graphClient = graphClient.ThrowIfNull();
    private readonly IOptionsSynchronizationService optionsSynchronizationService = optionsSynchronizationService.ThrowIfNull();

    private Dictionary<string, JsonDocument>? remoteOptions;
    private Dictionary<string, JsonDocument>? localOptions;

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
            : JsonSerializer.Serialize(this.localOptions, IndentedOptions);

        var remoteOptionsString = this.remoteOptions is null
            ? "{}"
            : JsonSerializer.Serialize(this.remoteOptions, IndentedOptions );

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
