using Daybreak.Shared.Services.Privilege;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Privilege;

// TODO: Implement Linux privilege management
// On Linux, privilege escalation typically uses pkexec/sudo
// For now, this is a no-op implementation that always reports non-admin
internal sealed class PrivilegeManager(
    ILogger<PrivilegeManager> logger) : IPrivilegeManager
{
    private readonly ILogger<PrivilegeManager> logger = logger;

    // TODO: Check if running as root (uid 0) or with specific capabilities
    public bool AdminPrivileges => false;

    public Task<bool> RequestAdminPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        // TODO: Implement privilege escalation via pkexec or polkit
        this.logger.LogWarning("RequestAdminPrivileges called but not implemented on Linux");
        return Task.FromResult(false);
    }

    public Task<bool> RequestNormalPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        // TODO: Implement privilege de-escalation
        this.logger.LogWarning("RequestNormalPrivileges called but not implemented on Linux");
        return Task.FromResult(false);
    }
}
