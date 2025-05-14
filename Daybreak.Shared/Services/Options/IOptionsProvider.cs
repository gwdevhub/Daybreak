using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Daybreak.Shared.Services.Options;

public interface IOptionsProvider
{
    JObject? TryGetKeyedOptions(string key);
    IEnumerable<object> GetRegisteredOptions();
    IEnumerable<Type> GetRegisteredOptionsTypes();
    void SaveRegisteredOptions(object options);
    void SaveRegisteredOptions(string name, JObject options);
}
