using Daybreak.Shared.Models;
using Daybreak.Shared.Services.DSOAL;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Registry;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;

namespace Daybreak.Services.DSOAL.Actions;

public sealed class FixSymbolicLinkStartupAction(
    INotificationService notificationService,
    IPrivilegeManager privilegeManager,
    IDSOALService dSOALService,
    IRegistryService registryService,
    ILogger<FixSymbolicLinkStartupAction> logger) : StartupActionBase
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IPrivilegeManager privilegeManager = privilegeManager.ThrowIfNull();
    private readonly IDSOALService dSOALService = dSOALService.ThrowIfNull();
    private readonly IRegistryService registryService = registryService.ThrowIfNull();
    private readonly ILogger<FixSymbolicLinkStartupAction> logger = logger.ThrowIfNull();

    public override void ExecuteOnStartup()
    {
        if (!this.registryService.TryGetValue<bool>(DSOALService.DSOALFixRegistryKey, out var shouldFix) ||
            !shouldFix)
        {
            return;
        }

        if (!this.privilegeManager.AdminPrivileges)
        {
            this.notificationService.NotifyError<FixSymbolicLinkNotificationHandler>(
                "DSOAL Error Detected",
                DSOALService.DSOALFixAdminMessage,
                expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));
            return;
        }

        this.dSOALService.EnsureDSOALSymbolicLinkExists();
        this.registryService.DeleteValue(DSOALService.DSOALFixRegistryKey);
        this.notificationService.NotifyInformation(
            "DSOAL Fixed",
            "DSOAL installation has been fixed",
            expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));
    }
}
