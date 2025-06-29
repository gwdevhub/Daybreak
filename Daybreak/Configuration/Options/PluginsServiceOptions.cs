using Daybreak.Attributes;
using Daybreak.Services.Plugins.Models;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class PluginsServiceOptions
{
    public List<PluginEntry> EnabledPlugins { get; set; } = [];
}
