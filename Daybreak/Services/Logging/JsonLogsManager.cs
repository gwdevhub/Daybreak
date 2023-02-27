using LiteDB;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging;

public sealed class JsonLogsManager : ILogsManager
{
    private readonly ILiteDatabase liteDatabase;

    public JsonLogsManager(ILiteDatabase liteDatabase)
    {
        this.liteDatabase = liteDatabase.ThrowIfNull(nameof(liteDatabase));
    }

    public IEnumerable<Models.Log> GetLogs(Expression<Func<Models.Log, bool>> filter)
    {
        return this.liteDatabase.GetCollection<Models.Log>().Find(filter);
    }
    public IEnumerable<Models.Log> GetLogs()
    {
        return this.liteDatabase.GetCollection<Models.Log>().FindAll();
    }
    public void WriteLog(Log log)
    {
        var dbLog = new Models.Log
        {
            EventId = log.EventId,
            Message = log.Exception is null ? log.Message : $"{log.Message}{Environment.NewLine}{log.Exception}",
            Category = log.Category,
            LogLevel = log.LogLevel,
            LogTime = log.LogTime,
            CorrelationVector = log.CorrelationVector
        };

        this.liteDatabase.GetCollection<Models.Log>().Insert(dbLog);
    }
    public int DeleteLogs()
    {
        return this.liteDatabase.GetCollection<Models.Log>().DeleteAll();
    }
}
