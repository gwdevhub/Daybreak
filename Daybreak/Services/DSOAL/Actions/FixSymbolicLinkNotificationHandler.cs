using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Views;
using System.Core.Extensions;

namespace Daybreak.Services.DSOAL.Actions;

public sealed class FixSymbolicLinkNotificationHandler : INotificationHandler
{
    private readonly IPrivilegeManager privilegeManager;

    public FixSymbolicLinkNotificationHandler(
        IPrivilegeManager privilegeManager)
    {
        this.privilegeManager = privilegeManager.ThrowIfNull();
    }

    public void OpenNotification(Notification notification)
    {
        this.privilegeManager.RequestAdminPrivileges<LauncherView>(DSOALService.DSOALFixAdminMessage);
    }
}
