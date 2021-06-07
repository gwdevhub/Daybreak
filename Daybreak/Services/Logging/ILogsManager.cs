using Daybreak.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using WpfExtended.Logging;

namespace Daybreak.Services.Logging
{
    public interface ILogsManager : ILogsWriter
    {
        IEnumerable<Log> GetLogs(Expression<Func<Log, bool>> filter);
        IEnumerable<Log> GetLogs();
        int DeleteLogs();
    }
}
