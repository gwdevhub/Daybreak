using System.Collections.Generic;
using System.Windows.Forms;

namespace Daybreak.Shared.Models;

public sealed class KeyMacro
{
    public List<Keys>? Keys { get; set; }
    public Keys TargetKey { get; set; }
}
