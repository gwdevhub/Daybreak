using Daybreak.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class JsonLogsManager : ILogsManager
{
    private readonly LinkedList<Models.Log> memoryCache = [];

    public event EventHandler<Models.Log>? ReceivedLog;

    public IEnumerable<Models.Log> GetLogs(Expression<Func<Models.Log, bool>> filter)
    {
        return this.memoryCache
            .Where(filter.Compile());
    }

    public IEnumerable<Models.Log> GetLogs()
    {
        return [.. this.memoryCache];
    }
    public void WriteLog(Log log)
    {
        var logModel = new Models.Log
        {
            EventId = log.EventId ?? string.Empty,
            Message = log.Exception is null ? log.Message : $"{log.Message}{Environment.NewLine}{log.Exception}",
            Category = log.Category,
            LogLevel = log.LogLevel,
            LogTime = log.LogTime.ToSafeDateTimeOffset(),
            CorrelationVector = log.CorrelationVector
        };

        this.memoryCache.AddLast(logModel);
        this.ReceivedLog?.Invoke(this, logModel);
    }
    public void DeleteLogs()
    {
        this.memoryCache.Clear();
    }
}
