using Microsoft.AspNetCore.Components;

namespace Daybreak.Shared.Services.Privilege;

public interface IPrivilegeManager
{
    bool AdminPrivileges { get; }

    Task<bool> RequestAdminPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase;

    Task<bool> RequestNormalPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase;
}
