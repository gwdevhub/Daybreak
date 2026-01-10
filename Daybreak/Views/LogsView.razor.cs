using Daybreak.Models;
using Daybreak.Services.Logging;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using Photino.NET;
using System.Extensions;
using System.IO.Compression;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;

public sealed class LogsViewModel(
    PhotinoWindow window,
    INotificationService notificationService,
    ILogger<LogsViewModel> logger)
    : ViewModelBase<LogsViewModel, LogsView>
{
    private const int MaxLogEntries = 200;

    private readonly PhotinoWindow window = window;
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogger<LogsViewModel> logger = logger;
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly List<StructuredLogEntry> logs = [];

    private bool isAttached = false;

    public IEnumerable<StructuredLogEntry> Logs
    {
        get
        {
            this.semaphore.Wait();
            try
            {
                return [.. this.logs];
            }
            finally
            {
                this.semaphore.Release();
            }
        }
    }

    public bool AutoScroll { get; private set; } = true;
    public bool Loading { get; private set; } = false;

    public override async ValueTask ParametersSet(LogsView view, CancellationToken cancellationToken)
    {
        await this.semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!this.isAttached)
            {
                InMemorySink.Instance.LogEventEmitted += this.Instance_LogEventEmitted;
                this.isAttached = true;
            }

            this.logs.ClearAnd().AddRange(InMemorySink.Instance.GetSnapshot().TakeLast(MaxLogEntries));
            await base.ParametersSet(view, cancellationToken);
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public void ClearLogs()
    {
        this.semaphore.Wait();
        try
        {
            this.logs.Clear();
        }
        finally
        {
            this.semaphore.Release();
        }

        this.RefreshView();
    }

    public void ToggleAutoScroll()
    {
        this.AutoScroll = !this.AutoScroll;
        this.RefreshView();
    }

    public async Task ArchiveLogs()
    {
        this.Loading = true;
        await this.RefreshViewAsync();
        var selectedFilePath = this.window.ShowSaveFileAsync("Save Logs", filters: [("Zip Files", [".zip"])]);
        try
        {
            var selectedFile = await selectedFilePath;
            if (string.IsNullOrWhiteSpace(selectedFile))
            {
                this.notificationService.NotifyInformation("Logs archive canceled", "Log archive was cancelled by user.");
                this.Loading = false;
                return;
            }

            using var fileStream = File.Open(selectedFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using var compressedStream = new GZipStream(fileStream, CompressionLevel.SmallestSize);
            using var streamWriter = new StreamWriter(compressedStream);
            streamWriter.Write(string.Join(Environment.NewLine, this.logs.ToArray()));
            streamWriter.Flush();
            this.notificationService.NotifyInformation("Logs archived", $"Logs were successfully archived to {selectedFile}");
        }
        catch(Exception ex)
        {
            this.notificationService.NotifyError("Logs archive failed", "An error occurred while archiving logs");
            this.logger.LogError(ex, "An error occurred while archiving logs");
        }
        finally
        {
            this.Loading = false;
        }
    }

    private void Instance_LogEventEmitted(object? sender, StructuredLogEntry e)
    {
        this.semaphore.Wait();
        try
        {
            this.logs.Add(e);
        }
        finally
        {
            this.semaphore.Release();
        }

        this.RefreshView();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            InMemorySink.Instance.LogEventEmitted -= this.Instance_LogEventEmitted;
            this.semaphore.Dispose();
        }

        base.Dispose(disposing);
    }
}
