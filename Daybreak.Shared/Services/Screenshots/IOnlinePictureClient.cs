using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Screenshots;

public interface IOnlinePictureClient
{
    Task<(ImageSource? Source, string Credit)> GetImage(bool localized);
}
