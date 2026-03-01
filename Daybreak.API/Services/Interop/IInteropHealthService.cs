using Daybreak.API.Models;

namespace Daybreak.API.Services.Interop;

public interface IInteropHealthService
{
    IEnumerable<AddressHealth> GetAddressHealth();
}
