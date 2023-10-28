using Daybreak.Models.Metrics;
using Daybreak.Services.Metrics;
using Daybreak.Services.Notifications;
using Daybreak.Services.UMod.Models;
using Ionic.Zip;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Core.Extensions;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Extensions;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod.Utilities;
internal sealed class UModClient : IUModClient
{
    private const string EntryLoadMetricName = "uMod loading latency";
    private const string EntryLoadMetricDescription = "The amount of milliseconds taken to load one entry of an uMod mod";
    private const string EntryLoadMetricUnit = "ms";
    private const int SMALL_PIPE_SIZE = 1 << 10;
    private const int BIG_PIPE_SIZE = 1 << 24;
    private const int MaxRetryCount = 10; //Equivalent of 5 seconds

    private readonly Histogram<double> latencyMetric;
    private readonly INotificationService notificationService;
    private readonly ILogger<UModClient> logger;

    private readonly List<TexLoader> texturePackLoaders = new();
    private readonly HashSet<uint> hashes = new();

    private NamedPipeServerStream? pipeReceive;
    private NamedPipeServerStream? pipeSend;
    private CachingStream? cachingStream;
    private bool receiveConnected;
    private bool sendConnected;

    public bool Ready => this.receiveConnected && this.sendConnected;

    public UModClient(
        IMetricsService metricsService,
        INotificationService notificationService,
        ILogger<UModClient> logger)
    {
        this.latencyMetric = metricsService.ThrowIfNull().CreateHistogram<double>(EntryLoadMetricName, EntryLoadMetricUnit, EntryLoadMetricDescription, AggregationTypes.NoAggregate);
        this.notificationService = notificationService.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async void Initialize(CancellationToken cancellationToken)
    {
        var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        var account = sid.Translate(typeof(NTAccount)).As<NTAccount>();
        var rule = new PipeAccessRule(account?.ToString()!, PipeAccessRights.FullControl, AccessControlType.Allow);
        var securityPipe = new PipeSecurity();
        securityPipe.AddAccessRule(rule);

        this.pipeReceive = NamedPipeServerStreamAcl.Create(
            "Game2uMod",
            PipeDirection.In,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.None,
            SMALL_PIPE_SIZE,
            SMALL_PIPE_SIZE,
            securityPipe);

        this.pipeSend = NamedPipeServerStreamAcl.Create(
            "uMod2Game",
            PipeDirection.Out,
            NamedPipeServerStream.MaxAllowedServerInstances,
            PipeTransmissionMode.Byte,
            PipeOptions.None,
            BIG_PIPE_SIZE,
            BIG_PIPE_SIZE,
            securityPipe);

        await this.BeginReceive(cancellationToken);
    }

    public async Task AddFile(string filePath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.AddFile), filePath);
        var fullPath = Path.GetFullPath(filePath);
        scopedLogger.LogInformation("Adding texture file");
        var texLoader = new TexLoader(fullPath);
        try
        {
            await texLoader.LoadAsync(cancellationToken);
            this.texturePackLoaders.Add(texLoader);
        }
        catch (OverflowException e)
        {
            scopedLogger.LogError(e, "Error adding texture file. Encountered exception");
            this.notificationService.NotifyError(
                title: $"uMod: Failed to add {filePath}",
                description: "Textures with 64 bit hashes are not supported. Please only add Texmods created with uMod r44 and under",
                expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));
        }
        catch (BadPasswordException e)
        {
            scopedLogger.LogError(e, $"Unable to load {Path.GetFileNameWithoutExtension(filePath)}. Bad password");
            this.notificationService.NotifyError(
                title: $"uMod: Failed to add {Path.GetFileNameWithoutExtension(filePath)}",
                description: $"Bad password while loading {filePath}. This probably means that the tpf file is invalid",
                expirationTime: DateTime.Now + TimeSpan.FromSeconds(5));
        }
    }

    public async Task WaitForInitialize(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.WaitForInitialize), string.Empty);
        var retryCount = 0;
        while (!this.Ready)
        {
            scopedLogger.LogInformation("Pipes are not yet ready. Waiting 100ms");
            await Task.Delay(500, cancellationToken);
            retryCount++;
            if (retryCount == MaxRetryCount)
            {
                scopedLogger.LogError($"Exceeded retry count. Throwing {nameof(TimeoutException)}");
                throw new TimeoutException("Timed out waiting for uMod connection");
            }
        }
    }

    public async Task<bool> Send(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.Send), string.Empty);
        await this.WaitForInitialize(cancellationToken);

        foreach(var loader in this.texturePackLoaders)
        {
            foreach(var entry in await loader.LoadAsync(cancellationToken))
            {
                if (this.hashes.Contains(entry.CrcHash))
                {
                    continue;
                }

                if (this.cachingStream!.Length + (2 * Marshal.SizeOf(typeof(TexmodMessage))) + entry.Entry!.UncompressedSize > BIG_PIPE_SIZE)
                {
                    var loadMore = new TexmodMessage(ControlMessage.CONTROL_MORE_TEXTURES, 0, 0);
                    this.AddMessage(loadMore, default);
                    if (!await this.SendAll(cancellationToken))
                    {
                        this.notificationService.NotifyError(
                            title: "uMod: Failed to send textures",
                            description: "Some of the queued textures failed to send. Please check logs for more information");
                        break;
                    }
                }

                var sw = Stopwatch.StartNew();
                using var reader = entry.Entry.OpenReader();
                var msg = new TexmodMessage(ControlMessage.CONTROL_ADD_TEXTURE_DATA, (uint)reader.Length, entry.CrcHash);
                this.hashes.Add(entry.CrcHash);
                this.AddMessage(msg, reader);
                this.latencyMetric.Record(sw.ElapsedMilliseconds);
            }

            scopedLogger.LogInformation($"Loaded {loader.Name}");
        }

        var success = await this.SendAll(cancellationToken);
        if (!success)
        {
            return false;
        }

        foreach(var loader in this.texturePackLoaders)
        {
            loader.Dispose();
        }

        this.texturePackLoaders.Clear();
        return true;
    }

    public void CloseConnection()
    {
        this.receiveConnected = false;
        this.sendConnected = false;
        this.cachingStream?.Dispose();
        this.cachingStream = null;
        this.pipeReceive?.Dispose();
        this.pipeReceive = null;
        this.pipeSend?.Dispose();
        this.pipeSend = null;
        foreach (var loader in this.texturePackLoaders)
        {
            loader.Dispose();
        }

        this.texturePackLoaders?.Clear();
        this.hashes?.Clear();
    }

    public void Dispose()
    {
        this.CloseConnection();
    }

    private async Task<bool> SendAll(CancellationToken cancellationToken)
    {
        while (this.pipeSend?.IsConnected is not true ||
               this.pipeSend?.CanWrite is not true ||
               this.Ready is not true)
        {
            this.logger.LogInformation("Send pipe is not yet initialized. Waiting 100ms");
            await Task.Delay(100, cancellationToken);
        }

        await this.cachingStream!.FlushAsync(cancellationToken);
        return true;
    }

    private void AddMessage(TexmodMessage msg, Stream? data)
    {
        this.cachingStream!.Write(BitConverter.GetBytes((uint)msg.Message));
        this.cachingStream!.Write(BitConverter.GetBytes(msg.Value));
        this.cachingStream!.Write(BitConverter.GetBytes(msg.Hash));
        if (data is Stream stream)
        {
            stream.CopyTo(this.cachingStream!);
        }
    }

    private async Task BeginReceive(CancellationToken cancellationToken)
    {
        if (this.pipeReceive is null)
        {
            throw new InvalidOperationException("Unexpected error. Receive pipe is null");
        }

        if (this.pipeSend is null)
        {
            throw new InvalidOperationException("Unexpected error. Send pipe is null");
        }

        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.BeginReceive), string.Empty);
        scopedLogger.LogInformation("Waiting for connection");
        await this.pipeReceive.WaitForConnectionAsync(cancellationToken);

        scopedLogger.LogInformation("Connected. Waiting for message");
        var buf = new byte[SMALL_PIPE_SIZE];
        var num = await this.pipeReceive.ReadAsync(buf, cancellationToken);
        this.receiveConnected = true;

        scopedLogger.LogDebug($"Message received. Byte count: {num}");
        if (num <= 2)
        {
            return;
        }

        var gameName = Encoding.Unicode.GetString(buf).Replace("\0", "");
        scopedLogger.LogDebug($"Received game name: {gameName}");
        if (!this.pipeSend.IsConnected)
        {
            scopedLogger.LogInformation("Attempting to initialize send pipe");
            await this.pipeSend.WaitForConnectionAsync(cancellationToken);
            this.cachingStream = new CachingStream(this.pipeSend, BIG_PIPE_SIZE);
            this.sendConnected = true;
            scopedLogger.LogInformation("Initialized send pipe");
        }
    }
}
