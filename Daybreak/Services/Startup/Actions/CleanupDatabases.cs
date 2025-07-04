﻿using Daybreak.Services.Notifications;
using Daybreak.Services.TradeChat;
using Daybreak.Shared.Models;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;

namespace Daybreak.Services.Startup.Actions;
internal sealed class CleanupDatabases(
    TradeQuoteDbContext quotesCollection,
    NotificationsDbContext notificationsCollection,
    TradeMessagesDbContext traderMessagesCollection,
    ILogger<CleanupDatabases> logger) : StartupActionBase
{
    private readonly TradeQuoteDbContext quotesCollection = quotesCollection.ThrowIfNull();
    private readonly NotificationsDbContext notificationsCollection = notificationsCollection.ThrowIfNull();
    private readonly TradeMessagesDbContext traderMessagesCollection = traderMessagesCollection.ThrowIfNull();
    private readonly ILogger<CleanupDatabases> logger = logger.ThrowIfNull();

    public override async Task ExecuteOnStartupAsync(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var notifications = await this.notificationsCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        if (notifications.Count > 1000)
        {
            await this.notificationsCollection.DeleteAll(cancellationToken);
            scopedLogger.LogDebug("Cleared notifications database");
        }

        var allQuotes = await this.quotesCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        if (allQuotes.Count > 20000)
        {
            // Delete the oldest 2000 entries in the db. We probably won't need them anymore
            var quotes = await this.quotesCollection.FindAll(cancellationToken).OrderBy(q => q.TimeStamp).Take(2000).ToListAsync(cancellationToken);
            foreach (var quote in quotes)
            {
                await this.quotesCollection.Delete(quote.Id, cancellationToken);
            }

            await this.quotesCollection.DeleteAll(cancellationToken);
            scopedLogger.LogDebug("Cleared quotes database");
        }

        var traderMessages = await this.traderMessagesCollection.FindAll(cancellationToken).ToListAsync(cancellationToken);
        if (traderMessages.Count > 1000)
        {
            await this.traderMessagesCollection.DeleteAll(cancellationToken);
            scopedLogger.LogDebug("Cleared trader messages database");
        }
    }
}
