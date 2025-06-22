using System;
using System.Diagnostics;

namespace Daybreak.Shared.Models;
public readonly struct DaybreakAPIContext(Uri apiUri, Process process)
{
    public readonly Uri ApiUri = apiUri;
    public readonly Process Process = process;
}
