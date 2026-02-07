using Daybreak.Shared.Services.Privilege;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Daybreak.Linux.Services.Privilege;

/// <summary>
/// Linux implementation of the privilege manager.
/// Since Linux relies on Wine, we assume admin privileges are always available and do not implement elevation logic.
/// </summary>
internal sealed class PrivilegeManager(
    ILogger<PrivilegeManager> logger) : IPrivilegeManager
{
    private readonly ILogger<PrivilegeManager> logger = logger;

    public bool AdminPrivileges => true;

    public Task<bool> RequestAdminPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        this.logger.LogWarning("AdminPrivileges not implemented on Linux.");
        return Task.FromResult(true);
    }

    public Task<bool> RequestNormalPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        this.logger.LogWarning("Admin privileges not implemented on Linux.");
        return Task.FromResult(true);
    }
}
