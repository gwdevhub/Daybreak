using Daybreak.Shared.Services.Logging;
using Daybreak.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Daybreak.Services.Logging;

internal sealed class JsonLogsManager : ILogsManager
{
    private readonly LinkedList<Shared.Models.Log> memoryCache = [];

    public event EventHandler<Shared.Models.Log>? ReceivedLog;

    public IEnumerable<Shared.Models.Log> GetLogs(Expression<Func<Shared.Models.Log, bool>> filter)
    {
        return this.memoryCache
            .Where(filter.Compile());
    }

    public IEnumerable<Shared.Models.Log> GetLogs()
    {
        return [.. this.memoryCache];
    }
    public void WriteLog(System.Logging.Log log)
    {
        var logModel = new Shared.Models.Log
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
