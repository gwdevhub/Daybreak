using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Daybreak.Models.Browser
{
    public class BrowserPayload
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum PayloadKeys
        {
            None,
            ContextMenu
        }

        [JsonProperty("Key")]
        public PayloadKeys Key { get; set; }
    }

    public sealed class BrowserPayload<T> : BrowserPayload
    {
        [JsonProperty("Value")]
        public T? Value { get; set; }
    }
}
