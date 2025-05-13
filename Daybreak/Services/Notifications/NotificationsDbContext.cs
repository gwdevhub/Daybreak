using Squealify;
using System.Data.Common;

namespace Daybreak.Services.Notifications;
public sealed class NotificationsDbContext(DbConnection connection) : NotificationDTOTableContextBase(connection)
{
}
