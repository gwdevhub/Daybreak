using Daybreak.Shared.Models.Options;
using Newtonsoft.Json.Linq;

namespace Daybreak.Shared.Services.Options;

public interface IOptionsProvider
{
    JObject? TryGetKeyedOptions(string key);
    IEnumerable<OptionInstance> GetRegisteredOptionInstances();
    OptionInstance GetRegisteredOptionInstance(string optionName);
    IEnumerable<OptionType> GetRegisteredOptionDefinitions();
    void SaveRegisteredOptions(object options);
    void SaveRegisteredOptions(string name, JObject options);
}
