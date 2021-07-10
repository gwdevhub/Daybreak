using Daybreak.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Logging;

namespace Daybreak.Services.Logging
{
    public interface ILogsManager : ILogsWriter
    {
        IEnumerable<Models.Log> GetLogs(Expression<Func<Models.Log, bool>> filter);
        IEnumerable<Models.Log> GetLogs();
        int DeleteLogs();
    }
}
