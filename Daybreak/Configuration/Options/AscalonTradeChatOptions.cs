﻿using Daybreak.Attributes;
using Daybreak.Shared.Configuration.Options;

namespace Daybreak.Configuration.Options;

[OptionsName(Name = "Ascalon Trade Chat")]
[OptionsIgnore]
[OptionsSynchronizationIgnore]
public sealed class AscalonTradeChatOptions : ITradeChatOptions
{
    public string HttpsUri { get; set; } = "https://ascalon.gwtoolbox.com/";

    public string WssUri { get; set; } = "wss://ascalon.gwtoolbox.com/";
}
