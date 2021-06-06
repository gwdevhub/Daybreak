using Daybreak.Models;
using System.Collections.Generic;
using WpfExtended.Logging;

namespace Daybreak.Services.Logging
{
    public interface ILogsManager : ILogsWriter
    {
        IEnumerable<Log> GetLogs();
        int DeleteLogs();
    }
}
