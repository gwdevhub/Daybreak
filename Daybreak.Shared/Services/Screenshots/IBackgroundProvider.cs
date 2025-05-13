using Daybreak.Models;
using System.Threading.Tasks;

namespace Daybreak.Services.Screenshots;

public interface IBackgroundProvider
{
    Task<BackgroundResponse> GetBackground();
}
