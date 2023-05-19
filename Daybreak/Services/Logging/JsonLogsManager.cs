using LiteDB;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Extensions;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging;

public sealed class JsonLogsManager : ILogsManager
{
    private readonly ILiteCollection<Models.Log> collection;

    public JsonLogsManager(ILiteCollection<Models.Log> collection)
    {
        this.collection = collection.ThrowIfNull();
    }

    public IEnumerable<Models.Log> GetLogs(Expression<Func<Models.Log, bool>> filter)
    {
        return this.collection.Find(filter);
    }
    public IEnumerable<Models.Log> GetLogs()
    {
        return this.collection.FindAll();
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

        this.collection.Insert(dbLog);
    }
    public int DeleteLogs()
    {
        return this.collection.DeleteAll();
    }
}
