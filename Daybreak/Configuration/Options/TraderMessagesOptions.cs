using Daybreak.Attributes;
using Daybreak.Services.TradeChat.Models;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
internal sealed class TraderMessagesOptions : ILiteCollectionOptions<TraderMessageDTO>
{
    public string CollectionName => "trader_messages";
}
