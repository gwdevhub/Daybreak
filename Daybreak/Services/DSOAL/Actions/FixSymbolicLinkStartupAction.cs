using Daybreak.Shared.Models;
using Daybreak.Shared.Services.DSOAL;
using Daybreak.Shared.Services.Notifications;
using Daybreak.Shared.Services.Privilege;
using Daybreak.Shared.Services.Registry;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;

namespace Daybreak.Services.DSOAL.Actions;

public sealed class FixSymbolicLinkStartupAction : StartupActionBase
{
    private readonly INotificationService notificationService;
    private readonly IPrivilegeManager privilegeManager;
    private readonly IDSOALService dSOALService;
    private readonly IRegistryService registryService;
    private readonly ILogger<FixSymbolicLinkStartupAction> logger;

    public FixSymbolicLinkStartupAction(
        INotificationService notificationService,
        IPrivilegeManager privilegeManager,
        IDSOALService dSOALService,
        IRegistryService registryService,
        ILogger<FixSymbolicLinkStartupAction> logger)
    {
        this.notificationService = notificationService.ThrowIfNull();
        this.privilegeManager = privilegeManager.ThrowIfNull();
        this.dSOALService = dSOALService.ThrowIfNull();
        this.registryService = registryService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

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
