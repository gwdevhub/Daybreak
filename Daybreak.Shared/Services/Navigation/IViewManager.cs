using System.Windows.Controls;

namespace Daybreak.Shared.Services.Navigation;

public interface IViewManager : IViewProducer
{
    void RegisterContainer(Panel panel);

    void ShowView<T>()
        where T : UserControl;

    void ShowView<T>(object dataContext)
        where T : UserControl;

    void ShowView(Type type);

    void ShowView(Type type, object dataContext);
}
