using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Logging Options")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class LoggingOptions : ILiteCollectionOptions<Models.Log>
{
    public string CollectionName => "logs";
}
