using System;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.SevenZip;
public interface ISevenZipExtractor
{
    Task<bool> ExtractToDirectory(string sourceFile, string destinationDirectory, Action<double, string> progressTracker, CancellationToken cancellationToken);
}
