using Daybreak.API.Models;

namespace Daybreak.API.Services.Interop;

public interface IAddressHealthService
{
    List<AddressState> GetAddressStates();
}
