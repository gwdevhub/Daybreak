using Daybreak.Models;
using Daybreak.Services.ApplicationLifetime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Daybreak.Services.Logging
{
    public interface ILoggingDatabase : IApplicationLifetimeService
    {
        Task<bool> ClearDatabase();
        Task<IEnumerable<Log>> GetLogsByDate(DateTime startTime, DateTime endTime);
        Task<IEnumerable<Log>> GetLogs();
        Task<bool> InsertLog(Log log);
    }
}
