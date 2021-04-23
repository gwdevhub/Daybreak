using System.Windows.Controls;

namespace Daybreak.Services.Privilege
{
    public interface IPrivilegeManager
    {
        bool AdminPrivileges { get; }

        void RequestAdminPrivileges<TCancelView>(string messageToUser, object dataContextOfCancelView = null)
            where TCancelView : UserControl;
    }
}
