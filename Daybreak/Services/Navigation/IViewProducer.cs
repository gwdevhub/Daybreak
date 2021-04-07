using System.Windows.Controls;

namespace Daybreak.Services.ViewManagement
{
    public interface IViewProducer
    {
        void RegisterView<T>()
            where T : UserControl;
    }
}
