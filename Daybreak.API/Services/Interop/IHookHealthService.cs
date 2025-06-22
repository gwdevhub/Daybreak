using Daybreak.API.Models;

namespace Daybreak.API.Services.Interop;

public interface IHookHealthService
{
    List<HookState> GetHookStates();
}
