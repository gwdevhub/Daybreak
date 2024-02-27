using Daybreak.Configuration.Options;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class JsonLogsManager : ILogsManager
{
    private const int MemoryCacheMaxSize = 5000;

    private readonly List<Models.Log> memoryCache = [];
    private readonly ILiteCollection<Models.Log> collection;
    private readonly ILiveOptions<LauncherOptions> liveOptions;

    public event EventHandler<Models.Log>? ReceivedLog;

    public JsonLogsManager(
        ILiteCollection<Models.Log> collection,
        ILiveOptions<LauncherOptions> liveOptions)
    {
        this.collection = collection.ThrowIfNull();
        this.liveOptions = liveOptions.ThrowIfNull();
    }

    public IEnumerable<Models.Log> GetLogs(Expression<Func<Models.Log, bool>> filter)
    {
        return this.collection.Find(filter).Concat(this.memoryCache.Where(filter.Compile()));
    }
    public IEnumerable<Models.Log> GetLogs()
    {
        return this.collection.FindAll().Concat(this.memoryCache);
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

        if (this.liveOptions.Value.PersistentLogging)
        {
            lock (this.memoryCache)
            {
                if (this.memoryCache.Count > 0)
                {
                    this.collection.InsertBulk(this.memoryCache, this.memoryCache.Count);
                    this.memoryCache.Clear();
                }
            }

            this.collection.Insert(dbLog);
        }
        else
        {
            lock(this.memoryCache)
            {
                if (this.memoryCache.Count >= MemoryCacheMaxSize)
                {
                    this.memoryCache.RemoveAt(this.memoryCache.Count - 1);
                }

                this.memoryCache.Add(dbLog);
            }
        }

        this.ReceivedLog?.Invoke(this, dbLog);
    }
    public int DeleteLogs()
    {
        this.memoryCache.Clear();
        return this.collection.DeleteAll();
    }
}
