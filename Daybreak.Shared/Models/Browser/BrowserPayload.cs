using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Daybreak.Shared.Models.Browser;

public class BrowserPayload
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PayloadKeys
    {
        None,
        ContextMenu,
        XButton1Pressed,
        XButton2Pressed
    }

    [JsonProperty(nameof(Key))]
    public PayloadKeys Key { get; set; }
}

public sealed class BrowserPayload<T> : BrowserPayload
{
    [JsonProperty(nameof(Value))]
    public T? Value { get; set; }
}
