using Daybreak.Services.Notifications;
using Daybreak.Services.TradeChat;
using Daybreak.Shared.Models;

namespace Daybreak.Services.Startup.Actions;
public sealed class EnsureDatabaseTablesExist(
    TradeQuoteDbContext tradeQuoteDbContext,
    TradeMessagesDbContext tradeMessagesDbContext,
    NotificationsDbContext notificationsDbContext)
    : StartupActionBase
{
    private readonly TradeQuoteDbContext tradeQuoteDbContext = tradeQuoteDbContext;
    private readonly TradeMessagesDbContext tradeMessagesDbContext = tradeMessagesDbContext;
    private readonly NotificationsDbContext notificationsDbContext = notificationsDbContext;

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        await this.tradeQuoteDbContext.CreateTableIfNotExists(cancellationToken);
        await this.tradeMessagesDbContext.CreateTableIfNotExists(cancellationToken);
        await this.notificationsDbContext.CreateTableIfNotExists(cancellationToken);
    }
}
