using Daybreak.Attributes;
using Daybreak.Services.Notifications.Models;

namespace Daybreak.Configuration.Options;

[OptionsIgnore]
[OptionsSynchronizationIgnore]
internal sealed class NotificationStorageOptions : ILiteCollectionOptions<NotificationDTO>
{
    public string CollectionName => "notifications";
}
