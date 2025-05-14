using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Shared.Services.Images;

public interface IImageCache
{
    Task<ImageSource?> GetImage(string? uri);
}
