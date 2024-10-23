using Daybreak.Services.Database;
using Daybreak.Services.Logging.Models;
using Daybreak.Services.Notifications.Models;
using Daybreak.Services.TradeChat.Models;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Linq;

namespace Daybreak.Services.Startup.Actions;
internal sealed class CleanupDatabases : StartupActionBase
{
    private readonly IDatabaseCollection<LogDTO> loggingCollection;
    private readonly IDatabaseCollection<TraderQuoteDTO> quotesCollection;
    private readonly IDatabaseCollection<NotificationDTO> notificationsCollection;
    private readonly IDatabaseCollection<TraderMessageDTO> traderMessagesCollection;
    private readonly ILogger<CleanupDatabases> logger;

    public CleanupDatabases(
        IDatabaseCollection<LogDTO> loggingCollection,
        IDatabaseCollection<TraderQuoteDTO> quotesCollection,
        IDatabaseCollection<NotificationDTO> notificationsCollection,
        IDatabaseCollection<TraderMessageDTO> traderMessagesCollection,
        ILogger<CleanupDatabases> logger)
    {
        this.loggingCollection = loggingCollection.ThrowIfNull();
        this.quotesCollection = quotesCollection.ThrowIfNull();
        this.notificationsCollection = notificationsCollection.ThrowIfNull();
        this.traderMessagesCollection = traderMessagesCollection.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public override void ExecuteOnStartup()
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.ExecuteOnStartup), string.Empty);
        if (this.loggingCollection.Count() > 50000)
        {
            this.loggingCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared logging database");
        }

        if (this.notificationsCollection.Count() > 1000)
        {
            this.notificationsCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared notifications database");
        }

        if (this.quotesCollection.Count() > 20000)
        {
            // Delete the oldest 2000 entries in the db. We probably won't need them anymore
            var quotes = this.quotesCollection.FindAll().OrderBy(q => q.TimeStamp).Take(2000).ToList();
            foreach(var quote in quotes)
            {
                this.quotesCollection.Delete(quote);
            }

            this.quotesCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared quotes database");
        }

        if (this.traderMessagesCollection.Count() > 1000)
        {
            this.traderMessagesCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared trader messages database");
        }
    }
}
