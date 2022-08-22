using Newtonsoft.Json;

namespace Daybreak.Services.Graph.Models;

public sealed class User
{
    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("mail")]
    public string Email { get; set; }
}
