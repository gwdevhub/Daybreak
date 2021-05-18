using Daybreak.Models;
using System.Collections.Generic;

namespace Daybreak.Services.Logging
{
    public interface ILogsManager
    {
        void WriteLog(Log log);
        IEnumerable<Log> GetLogs();
        int DeleteLogs();
    }
}
