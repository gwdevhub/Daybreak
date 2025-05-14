using System.Collections.Generic;

namespace Daybreak.Shared.Models.Browser;
public sealed class BrowserHistory
{
    public List<string> History { get; set; } = [];
    public int CurrentPosition { get; set; } = -1;
}
