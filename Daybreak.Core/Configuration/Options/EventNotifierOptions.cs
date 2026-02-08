using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Event Notifier")]
internal sealed class EventNotifierOptions
{
    [OptionName(Name = "Enabled", Description = "If set to true, Daybreak will notify the user of any event on startup")]
    public bool Enabled { get; set; } = true;
}
