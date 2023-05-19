using Daybreak.Attributes;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Logging Options")]
[OptionsIgnore]
public sealed class LoggingOptions : ILiteCollectionOptions<Models.Log>
{
    public string CollectionName => "logs";
}
