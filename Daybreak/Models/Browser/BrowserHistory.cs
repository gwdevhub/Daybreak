using System.Collections.Generic;

namespace Daybreak.Models.Browser;
public sealed class BrowserHistory
{
    public List<string> History { get; set; } = [];
    public int CurrentPosition { get; set; } = -1;
}
