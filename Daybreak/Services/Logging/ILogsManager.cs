using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging;

public interface ILogsManager : ILogsWriter
{
    event EventHandler<Daybreak.Models.Log>? ReceivedLog;

    IEnumerable<Daybreak.Models.Log> GetLogs(Expression<Func<Daybreak.Models.Log, bool>> filter);
    IEnumerable<Daybreak.Models.Log> GetLogs();
    void DeleteLogs();
}
