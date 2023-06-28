using System.Threading.Tasks;
using System.Windows.Media;

namespace Daybreak.Services.Bloogum;

public interface IBloogumClient
{
    Task<ImageSource?> GetImage(bool localized);
}
