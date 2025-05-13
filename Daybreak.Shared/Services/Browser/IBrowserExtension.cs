using System.Threading.Tasks;

namespace Daybreak.Services.Browser;

public interface IBrowserExtension
{
    string ExtensionId { get; }
    Task CheckAndUpdate(string browserVersion);
    Task<string> GetExtensionPath();
}
