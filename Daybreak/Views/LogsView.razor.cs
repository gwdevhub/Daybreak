using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Logging;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Extensions;
using System.IO;
using System.IO.Compression;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class LogsViewModel(
    INotificationService notificationService,
    ILogsManager logsManager,
    ILogger<LogsViewModel> logger)
    : ViewModelBase<LogsViewModel, LogsView>
{
    private readonly INotificationService notificationService = notificationService;
    private readonly ILogsManager logsManager = logsManager;
    private readonly ILogger<LogsViewModel> logger = logger;
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly List<Log> logs = [];

    private bool attached;

    public IEnumerable<Log> Logs
    {
        get
        {
            this.semaphore.Wait();
            try
            {
                return [.. this.logs.Where(l => l is not null && l.LogLevel > Microsoft.Extensions.Logging.LogLevel.Debug && l.LogLevel != Microsoft.Extensions.Logging.LogLevel.None)];
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
        if (!this.attached)
        {
            this.logsManager.ReceivedLog += this.LogsManager_ReceivedLog;
            this.attached = true;
        }

        await this.semaphore.WaitAsync(cancellationToken);
        try
        {
            this.logs.ClearAnd().AddRange(this.logsManager.GetLogs().Where(l => l is not null && l.LogLevel > Microsoft.Extensions.Logging.LogLevel.Debug && l.LogLevel != Microsoft.Extensions.Logging.LogLevel.None));
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
        var serializedLogsTask = Task.Factory.StartNew(() =>
        {
            var logs = this.logsManager.GetLogs();
            return JsonConvert.SerializeObject(logs, Formatting.Indented);
        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);

        var selectedFilePath = Task.Factory.StartNew(() =>
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = "Save Logs",
                Filter = "Zip Files (*.zip)|*.zip|All Files (*.*)|*.*",
                DefaultExt = "zip",
                AddExtension = true,
                OverwritePrompt = true,
                FileName = $"logs-{DateTime.Now:yyyyMMdd-HHmmss}.zip",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            };

            if (saveFileDialog.ShowDialog() is true)
            {
                return saveFileDialog.FileName;
            }

            return default;
        });

        try
        {
            var serializedLogs = await serializedLogsTask;
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
            streamWriter.Write(serializedLogs);
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

    private void LogsManager_ReceivedLog(object? sender, Log e)
    {
        if (e is null ||
            e.LogLevel <= LogLevel.Debug ||
            e.LogLevel == LogLevel.None)
        {
            return;
        }

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
            this.logsManager.ReceivedLog -= this.LogsManager_ReceivedLog;
            this.semaphore.Dispose();
        }

        base.Dispose(disposing);
    }
}
