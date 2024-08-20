using Daybreak.Services.Notifications.Models;
using Daybreak.Services.TradeChat.Models;
using LiteDB;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.Startup.Actions;
public sealed class CleanupDatabases : StartupActionBase
{
    private readonly ILiteCollection<Models.Log> loggingCollection;
    private readonly ILiteCollection<TraderQuoteDTO> quotesCollection;
    private readonly ILiteCollection<NotificationDTO> notificationsCollection;
    private readonly ILiteCollection<TraderMessageDTO> traderMessagesCollection;
    private readonly ILogger<CleanupDatabases> logger;

    public CleanupDatabases(
        ILiteCollection<Models.Log> loggingCollection,
        ILiteCollection<TraderQuoteDTO> quotesCollection,
        ILiteCollection<NotificationDTO> notificationsCollection,
        ILiteCollection<TraderMessageDTO> traderMessagesCollection,
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
        if (this.loggingCollection.LongCount() > 50000)
        {
            this.loggingCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared logging database");
        }

        if (this.notificationsCollection.LongCount() > 1000)
        {
            this.notificationsCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared notifications database");
        }

        if (this.quotesCollection.LongCount() > 1000)
        {
            this.quotesCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared quotes database");
        }

        if (this.traderMessagesCollection.LongCount() > 1000)
        {
            this.traderMessagesCollection.DeleteAll();
            scopedLogger.LogInformation("Cleared trader messages database");
        }
    }
}
