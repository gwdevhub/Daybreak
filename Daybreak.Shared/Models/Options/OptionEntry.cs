using System.Windows.Controls;

namespace Daybreak.Shared.Models.Options;
public sealed class OptionEntry
{
    public OptionHeading Heading { get; init; } = default!;
    public UserControl Template { get; init; } = default!;
    public OptionSetter Setter { get; init; } = default!;
}
