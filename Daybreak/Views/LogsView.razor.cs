using Daybreak.Shared.Models;
using Daybreak.Shared.Services.Logging;
using System.Extensions;
using TrailBlazr.ViewModels;

namespace Daybreak.Views;
public sealed class LogsViewModel(ILogsManager logsManager)
    : ViewModelBase<LogsViewModel, LogsView>
{
    private readonly ILogsManager logsManager = logsManager;
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

    private void LogsManager_ReceivedLog(object? sender, Log e)
    {
        if (e is null ||
            e.LogLevel <= Microsoft.Extensions.Logging.LogLevel.Debug ||
            e.LogLevel == Microsoft.Extensions.Logging.LogLevel.None)
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
