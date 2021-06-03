using System.Collections.Generic;
using WpfExtended.Logging;
using WpfExtended.Models;

namespace Daybreak.Services.Logging
{
    public interface ILogsManager : ILogsWriter
    {
        IEnumerable<Log> GetLogs();
        int DeleteLogs();
    }
}
