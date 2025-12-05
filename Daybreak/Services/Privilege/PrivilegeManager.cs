using Daybreak.Shared.Services.Privilege;
using Daybreak.Views;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Core.Extensions;
using System.Extensions;
using System.Security.Principal;
using TrailBlazr.Services;

namespace Daybreak.Services.Privilege;

internal sealed class PrivilegeManager(
    PrivilegeContext privilegeContext,
    IViewManager viewManager,
    ILogger<PrivilegeManager> logger) : IPrivilegeManager
{
    public bool AdminPrivileges => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    private readonly PrivilegeContext privilegeContext = privilegeContext.ThrowIfNull();
    private readonly IViewManager viewManager = viewManager.ThrowIfNull();
    private readonly ILogger<PrivilegeManager> logger = logger.ThrowIfNull();

    public Task<bool> RequestAdminPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();
        cancellationToken.Register(() =>
        {
            taskCompletionSource.TrySetCanceled();
            this.viewManager.ShowView<TCancelView>(cancelViewParams ?? []);
        });
        this.privilegeContext.PrivilegeRequestOperation = taskCompletionSource;
        this.privilegeContext.UserMessage = messageToUser;
        this.privilegeContext.CancelViewType = typeof(TCancelView);
        this.privilegeContext.CancelViewParams = cancelViewParams;
        this.viewManager.ShowView<RequestElevationView>();
        return taskCompletionSource.Task;
    }

    public Task<bool> RequestNormalPrivileges<TCancelView>(string messageToUser, (string, object)[]? cancelViewParams, CancellationToken cancellationToken)
        where TCancelView : ComponentBase
    {
        var taskCompletionSource = new TaskCompletionSource<bool>();
        cancellationToken.Register(() =>
        {
            taskCompletionSource.TrySetCanceled();
            this.viewManager.ShowView<TCancelView>(cancelViewParams ?? []);
        });
        this.privilegeContext.PrivilegeRequestOperation = taskCompletionSource;
        this.privilegeContext.UserMessage = messageToUser;
        this.privilegeContext.CancelViewType = typeof(TCancelView);
        this.privilegeContext.CancelViewParams = cancelViewParams;
        this.viewManager.ShowView<RequestDelevationView>();
        return taskCompletionSource.Task;
    }
}
