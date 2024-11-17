using Daybreak.Configuration.Options;
using Daybreak.Services.Database;
using Daybreak.Services.Logging.Models;
using Daybreak.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Linq.Expressions;
using System.Logging;
using System.Threading;

namespace Daybreak.Services.Logging;

internal sealed class JsonLogsManager : ILogsManager
{
    private const int MemoryCacheMaxSize = 5000;

    private readonly List<Daybreak.Models.Log> memoryCache = [];
    private readonly SemaphoreSlim semaphoreSlim = new(1);
    private readonly IDatabaseCollection<LogDTO> collection;
    private readonly ILiveOptions<LauncherOptions> liveOptions;

    public event EventHandler<Daybreak.Models.Log>? ReceivedLog;

    public JsonLogsManager(
        IDatabaseCollection<LogDTO> collection,
        ILiveOptions<LauncherOptions> liveOptions)
    {
        this.collection = collection.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
    }

    public IEnumerable<Daybreak.Models.Log> GetLogs(Expression<Func<Daybreak.Models.Log, bool>> filter)
    {
        return this.collection.FindAll().Select(log => new Daybreak.Models.Log
        {
            Category = log.Category,
            CorrelationVector = log.CorrelationVector,
            EventId = log.EventId,
            Message = log.Message,
            LogLevel = (LogLevel)log.LogLevel,
            LogTime = log.LogTime
        }).Where(filter.Compile());
    }
    public IEnumerable<Daybreak.Models.Log> GetLogs()
    {
        return this.collection.FindAll().Select(log => new Daybreak.Models.Log
        {
            Category = log.Category,
            CorrelationVector = log.CorrelationVector,
            EventId = log.EventId,
            Message = log.Message,
            LogLevel = (LogLevel)log.LogLevel,
            LogTime = log.LogTime
        });
    }
    public async void WriteLog(Log log)
    {
        var logModel = new Daybreak.Models.Log
        {
            EventId = log.EventId ?? string.Empty,
            Message = log.Exception is null ? log.Message : $"{log.Message}{Environment.NewLine}{log.Exception}",
            Category = log.Category,
            LogLevel = log.LogLevel,
            LogTime = log.LogTime.ToSafeDateTimeOffset(),
            CorrelationVector = log.CorrelationVector
        };

        if (this.liveOptions.Value.PersistentLogging)
        {
            using var context = await this.semaphoreSlim.Acquire();
            if (this.memoryCache.Count > 100)
            {
                this.collection.AddBulk(this.memoryCache.Select(l => new LogDTO
                {
                    EventId = l.EventId,
                    Message = l.Message,
                    Category = l.Category,
                    LogLevel = (int)l.LogLevel,
                    LogTime = l.LogTime,
                    CorrelationVector = l.CorrelationVector
                }));

                this.memoryCache.Clear();
            }

            this.memoryCache.Add(logModel);
        }
        else
        {
            using var context = await this.semaphoreSlim.Acquire();
            if (this.memoryCache.Count >= MemoryCacheMaxSize)
            {
                this.memoryCache.RemoveAt(this.memoryCache.Count - 1);
            }

            this.memoryCache.Add(logModel);
        }

        this.ReceivedLog?.Invoke(this, logModel);
    }
    public void DeleteLogs()
    {
        this.memoryCache.Clear();
        this.collection.DeleteAll();
    }
}
