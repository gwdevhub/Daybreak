using System.Text.Json.Serialization;

namespace Daybreak.Shared.Models.Api;

[JsonConverter(typeof(JsonStringEnumConverter<LanguageInfo>))]
public enum LanguageInfo
{
    English,
    Korean,
    French,
    German,
    Italian,
    Spanish,
    TraditionalChinese,
    Japanese = 8,
    Polish,
    Russian,
    BorkBorkBork = 17,
    Unknown = 0xff
}
