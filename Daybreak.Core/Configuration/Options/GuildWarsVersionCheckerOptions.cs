using Daybreak.Shared.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
public sealed class GuildWarsVersionCheckerOptions
{
    public bool IsEnabled { get; set; } = true;
}
