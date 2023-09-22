using Daybreak.Services.UMod.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod.Utilities;
internal sealed class TexLoader : IDisposable
{
    private static readonly byte[] TpfPasswordBytes =
    {
        0x73, 0x2A, 0x63, 0x7D, 0x5F, 0x0A, 0xA6, 0xBD,
        0x7D, 0x65, 0x7E, 0x67, 0x61, 0x2A, 0x7F, 0x7F,
        0x74, 0x61, 0x67, 0x5B, 0x60, 0x70, 0x45, 0x74,
        0x5C, 0x22, 0x74, 0x5D, 0x6E, 0x6A, 0x73, 0x41,
        0x77, 0x6E, 0x46, 0x47, 0x77, 0x49, 0x0C, 0x4B,
        0x46, 0x6F
    };

    private static readonly string TpfPassword = ZipFile.DefaultEncoding.GetString(TpfPasswordBytes);

    private readonly object lockObject = new();
    private readonly string filePath;
    private readonly XORStream stream;
    private readonly List<TpfEntry> entryCache = new();

    private bool loaded = false;

    public string Name { get; }

    public TexLoader(string fileName)
    {
        this.filePath = Path.GetFullPath(fileName);
        if (!File.Exists(this.filePath))
        {
            throw new InvalidOperationException($"File does not exist: {this.filePath}");
        }

        this.stream = new XORStream(new FileStream(fileName, FileMode.Open));
        this.Name = Path.GetFileNameWithoutExtension(this.filePath);
    }

    public Task<List<TpfEntry>> LoadAsync(CancellationToken cancellationToken = default)
    {
        /*
         * Queue this job as a task, as parsing
         * and extracting the tpf archive can take a lot of time.
         * This way, the UI thread can stay responsive.
         */
        return Task.Run(() =>
        {
            /*
             * Make sure that we parse the contents only once.
             * If the content has already been parsed, return the cache.
             */
            while (!Monitor.TryEnter(this.lockObject)) { }

            if (!this.loaded)
            {
                this.entryCache.AddRange(this.GetContents());
                this.loaded = true;
            }

            Monitor.Exit(this.lockObject);
            return this.entryCache;
        }, cancellationToken);
    }

    private IEnumerable<TpfEntry> GetContents()
    {
        /*
         * Extract the tpf, obtaining a list of entries and streams.
         * If the archive contains a texmod.def file, use that to obtain the
         * definition of the tpf.
         * Loop over the names and adjust them for uMod.
         */
        var files = new Dictionary<string, ZipEntry>();
        if (Path.GetExtension(this.filePath) == ".tpf")
        {
            files.AddRange(this.GetTpfContents().Select(tuple => new KeyValuePair<string, ZipEntry>(tuple.Name, tuple.ZipEntry)));
        }
        else
        {
            files.AddRange(this.GetFileContents().Select(tuple => new KeyValuePair<string, ZipEntry>(tuple.Name, tuple.ZipEntry)));
        }

        if (files.TryGetValue("texmod.def", out var texContentEntry))
        {
            using var stream = texContentEntry.OpenReader();
            using var reader = new StreamReader(stream, Encoding.Default);
            var text = reader.ReadToEnd();
            var lines = text.Replace("\r", "").Split('\n');
            return GetTextureContents(files, lines).Select(tuple => new TpfEntry { Name = tuple.Name, Entry = tuple.ZipEntry, CrcHash = Convert.ToUInt32(tuple.Name, 16) });
        }

        return GetTextureContents(files).Select(tuple => new TpfEntry { Name = tuple.Name, Entry = tuple.ZipEntry, CrcHash = Convert.ToUInt32(tuple.Name, 16) });
    }

    private IEnumerable<(string Name, ZipEntry ZipEntry)> GetTpfContents()
    {
        using var archive = ZipFile.Read(this.stream);
        archive.Password = TpfPassword;
        archive.Encryption = EncryptionAlgorithm.None;

        foreach (var entry in archive.Entries)
        {
            yield return (entry.FileName, entry);
        }
    }

    private IEnumerable<(string Name, ZipEntry ZipEntry)> GetFileContents()
    {
        using var archive = ZipFile.Read(this.stream);
        foreach (var entry in archive.Entries)
        {
            yield return (entry.FileName, entry);
        }
    }

    private static IEnumerable<(string Name, ZipEntry ZipEntry)> GetTextureContents(Dictionary<string, ZipEntry> files)
    {
        foreach (var file in files)
        {
            // GW.EXE_0x12345678.dds
            (var fileName, var content) = file;
            if (content == null)
            {
                continue;
            }

            var name = fileName;
            while (name.Contains('_'))
            {
                var firstIndex = name.LastIndexOf('_');
                name = ++firstIndex >= fileName.Length - 1 ? fileName : fileName[firstIndex..];
            }

            if (name.Contains('.'))
            {
                var lastIndex = name.LastIndexOf('.');
                name = name[..lastIndex];
            }

            // 0x18F22DA3
            var crc = name;
            yield return (crc, content);
        }
    }

    private static IEnumerable<(string Name, ZipEntry ZipEntry)> GetTextureContents(Dictionary<string, ZipEntry> files, string[] definition)
    {
        foreach (var line in definition)
        {
            var splits = line.Split('|');
            if (splits.Length != 2)
            {
                continue;
            }

            var addrstr = splits[0];
            var path = splits[1];
            while (path[0] == '.' && (path[1] == '/' || path[1] == '\\') || path[0] == '/' || path[0] == '\\')
            {
                path = path.Remove(0, 1);
            }

            if (!files.ContainsKey(path))
            {
                continue;
            }

            files.Remove(path, out var content);
            if (content is null)
            {
                continue;
            }

            yield return (addrstr, content);
        }
    }

    public void Dispose()
    {
        this.entryCache.Clear();
        this.stream?.Dispose();
    }
}
