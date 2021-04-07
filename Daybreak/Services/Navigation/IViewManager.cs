using System.Windows.Controls;

namespace Daybreak.Services.ViewManagement
{
    public interface IViewManager : IViewProducer
    {
        void RegisterContainer(Panel panel);

        void ShowView<T>()
            where T : UserControl;
    }
}
