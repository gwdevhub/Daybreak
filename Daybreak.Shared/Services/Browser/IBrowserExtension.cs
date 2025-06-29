namespace Daybreak.Shared.Services.Browser;

public interface IBrowserExtension
{
    string ExtensionId { get; }
    Task CheckAndUpdate(string browserVersion);
    Task<string> GetExtensionPath();
}
