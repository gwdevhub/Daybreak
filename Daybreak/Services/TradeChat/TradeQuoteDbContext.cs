using Squealify;
using System.Data.Common;

namespace Daybreak.Services.TradeChat;
public sealed class TradeQuoteDbContext(DbConnection connection) : TraderQuoteDTOTableContextBase(connection)
{
    public ValueTask<DbTransaction> CreateTransaction(CancellationToken cancellationToken) => this.Connection.BeginTransactionAsync(cancellationToken);
}
