namespace Daybreak.Shared.Services.SevenZip;
public interface ISevenZipExtractor
{
    Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken);
}
