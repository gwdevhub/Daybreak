using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Shared.Services.Screenshots;

public interface IOnlinePictureClient
{
    Task<(ImageSource? Source, string Credit)> GetImage(bool localized);
}
