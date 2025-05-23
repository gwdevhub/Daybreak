﻿using Daybreak.Shared.Services.Logging;
using System.Diagnostics;
using System.Logging;

namespace Daybreak.Services.Logging;

internal sealed class DebugLogsWriter : IDebugLogsWriter
{
    public void WriteLog(Log log)
    {
        Debug.WriteLine($"[{log.LogTime}]\t[{log.LogLevel}]\t[{log.Category}]\n{log.Message}");
    }
}
