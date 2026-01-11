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
    void SaveRegisteredOptions(OptionInstance optionInstance);
    void SaveRegisteredOptions(string name, JObject options);
    void SaveOption<TOptions>(TOptions options)
        where TOptions : notnull;
}
