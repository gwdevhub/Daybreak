using Daybreak.Services.Screenshots.Models;
using System.Threading.Tasks;

namespace Daybreak.Services.Screenshots;

public interface IBackgroundProvider
{
    Task<BackgroundResponse> GetBackground();
}
