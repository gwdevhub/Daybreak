/*
 * Extractor is in a different executable because of memory limitations. Daybreak needs to be compiled for x86 and as such has a memory limit of 2GB.
 * 7Zip algorithm may require more than 2GB memory to unpack large zip files.
 * As such, Daybreak.7ZipExtractor is x64.
 */
using SharpCompress.Archives;

if (args.Length != 2)
{
    return -1;
}

var archivePath = args[0];
var destinationDirectory = args[1];

using var fileStream = new FileStream(archivePath, FileMode.Open);
using var archive = ArchiveFactory.Open(fileStream);
var progress = 0d;
var count = archive.Entries.Count();
var reader = archive.ExtractAllEntries();
while (reader.MoveToNextEntry())
{
    var entry = reader.Entry;
    if (entry.IsDirectory)
    {
        var directoryName = Path.Combine(Path.GetFullPath(destinationDirectory), entry.Key);
        Directory.CreateDirectory(directoryName);
        continue;
    }

    var fileName = Path.Combine(Path.GetFullPath(destinationDirectory), entry.Key);
    using var entryStream = new FileStream(fileName, FileMode.Create);
    using (var stream = reader.OpenEntryStream())
    {
        stream.CopyTo(entryStream);
    }

    progress += 1d / count;
    Console.WriteLine($"{progress} {fileName}");
}

return 0;
