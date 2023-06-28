using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Images;

public interface IImageCache
{
    Task<ImageSource?> GetImage(string? uri);
}
