using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

public interface IScreenshotProvider
{
    Task<ImageSource?> GetRandomScreenShot();
}
