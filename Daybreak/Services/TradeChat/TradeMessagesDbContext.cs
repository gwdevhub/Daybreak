using Squealify;
using System.Data.Common;

namespace Daybreak.Services.TradeChat;
public sealed class TradeMessagesDbContext(DbConnection connection) : TraderMessageDTOTableContextBase(connection)
{
}
