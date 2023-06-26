﻿using Daybreak.Attributes;
using Daybreak.Services.TradeChat.Models;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
public sealed class TraderMessagesOptions : ILiteCollectionOptions<TraderMessageDTO>
{
    public string CollectionName => "trader_messages";
}
