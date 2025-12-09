using Daybreak.Shared.Models.Notifications;
using Daybreak.Shared.Models.Notifications.Handling;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Views;
using System.Core.Extensions;

namespace Daybreak.Services.DSOAL.Actions;

//TODO: Remove once DSOAL is removed
public sealed class FixSymbolicLinkNotificationHandler(
    IPrivilegeManager privilegeManager) : INotificationHandler
{
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();

    public void OpenNotification(Notification notification)
    {
        Task.Run(() => this.privilegeManager.RequestAdminPrivileges<LaunchView>(DSOALService.DSOALFixAdminMessage, default, CancellationToken.None));
    }
}
