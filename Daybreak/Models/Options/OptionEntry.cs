using System.Windows.Controls;

namespace Daybreak.Models.Options;
public sealed class OptionEntry
{
    public OptionHeading Heading { get; init; } = default!;
    public UserControl Template { get; init; } = default!;
}
