using Daybreak.Models;
using LiteDB;
using System.Collections.Generic;
using System.Extensions;

namespace Daybreak.Services.Logging
{
    public sealed class JsonLogsManager : ILogsManager
    {
        private readonly ILiteDatabase liteDatabase;

        public JsonLogsManager(ILiteDatabase liteDatabase)
        {
            this.liteDatabase = liteDatabase.ThrowIfNull(nameof(liteDatabase));
        }

        public IEnumerable<Log> GetLogs()
        {
            return this.liteDatabase.GetCollection<Log>().FindAll();
        }
        public void WriteLog(Log log)
        {
            this.liteDatabase.GetCollection<Log>().Insert(log);
        }
        public int DeleteLogs()
        {
            return this.liteDatabase.GetCollection<Log>().DeleteAll();
        }
    }
}
