using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Daybreak.Services.Options;

public interface IOptionsProvider
{
    IEnumerable<object> GetRegisteredOptions();
    IEnumerable<Type> GetRegisteredOptionsTypes();
    void SaveRegisteredOptions(object options);
    void SaveRegisteredOptions(string name, JObject options);
}
