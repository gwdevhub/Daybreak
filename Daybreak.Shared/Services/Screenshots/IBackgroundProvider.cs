using Daybreak.Shared.Models;
using System.Threading.Tasks;

namespace Daybreak.Shared.Services.Screenshots;

public interface IBackgroundProvider
{
    Task<BackgroundResponse> GetBackground();
}
