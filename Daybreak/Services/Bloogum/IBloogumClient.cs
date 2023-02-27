using System.Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Daybreak.Services.Bloogum;

public interface IBloogumClient
{
    Task<Optional<Stream>> GetRandomScreenShot();
}
