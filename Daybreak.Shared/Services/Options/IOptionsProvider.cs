using Daybreak.Shared.Models.Options;
using System.Text.Json;

namespace Daybreak.Shared.Services.Options;

public interface IOptionsProvider
{
    JsonDocument? TryGetKeyedOptions(string key);
    IEnumerable<OptionInstance> GetRegisteredOptionInstances();
    OptionInstance GetRegisteredOptionInstance(string optionName);
    IEnumerable<OptionType> GetRegisteredOptionDefinitions();
    void SaveRegisteredOptions(object options);
    void SaveRegisteredOptions(OptionInstance optionInstance);
    void SaveRegisteredOptions(string name, JsonDocument options);
    void SaveOption<TOptions>(TOptions options)
        where TOptions : notnull;
}
