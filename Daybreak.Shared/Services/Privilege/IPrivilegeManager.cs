using System.Windows.Controls;

namespace Daybreak.Shared.Services.Privilege;

public interface IPrivilegeManager
{
    bool AdminPrivileges { get; }

    void RequestAdminPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = null)
        where TCancelView : UserControl;

    void RequestNormalPrivileges<TCancelView>(string messageToUser, object? dataContextOfCancelView = default)
        where TCancelView : UserControl;
}
