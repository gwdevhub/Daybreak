using Daybreak.Services.UMod.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.UMod.Utilities;

internal sealed class UModClient : IUModClient
{
    private const int MinPort = 42091;
    private const int MaxPort = 45000;
    private const string UrlTemplate = "http://127.0.0.1:[PORT]";
    private const string PortPlaceholder = "[PORT]";

    private static readonly (int MinRange, int MaxRange) PortRange = (MinPort, MaxPort);

    private readonly IHttpClient<UModClient> httpClient;
    private readonly ILogger<UModClient> logger;

    private int? cachedPort;

    public UModClient(
        IHttpClient<UModClient> httpClient,
        ILogger<UModClient> logger)
    {
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<UModConnectionContext?> Initialize(Process process, CancellationToken cancellationToken)
    {
        process.ThrowIfNull();
        var listeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
            .Where(i => i.Port >= PortRange.MinRange && i.Port < PortRange.MaxRange && i.Address.ToString() == IPAddress.Any.ToString());
        foreach (var listener in listeners)
        {
            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.Initialize), listener.Port.ToString());
            try
            {
                var response = await this.httpClient.GetAsync($"{UrlTemplate.Replace(PortPlaceholder, listener.Port.ToString())}/id", cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    scopedLogger.LogInformation($"Received non-success status code. Continuing");
                    continue;
                }

                var id = await response.Content.ReadAsStringAsync(cancellationToken);
                if (!int.TryParse(id, out var processId))
                {
                    scopedLogger.LogInformation("Received response is not an integer. Continuing");
                    continue;
                }

                if (processId != process.Id)
                {
                    scopedLogger.LogInformation("Received response does not match desired process id. Continuing");
                    continue;
                }

                return new UModConnectionContext { Port = (uint)listener.Port };
            }
            catch (Exception e) when (e is TaskCanceledException or TimeoutException)
            {
                scopedLogger.LogInformation($"Timed out trying to reach port. Continuing");
            }
        }

        return default;
    }

    public async Task AddFile(string filePath, UModConnectionContext uModConnectionContext, CancellationToken cancellationToken)
    {
        filePath.ThrowIfNull();
        filePath = Path.GetFullPath(filePath);
        var response = await this.httpClient.GetAsync($"{UrlTemplate.Replace(PortPlaceholder, uModConnectionContext.Port.ToString())}/add?name={filePath}", cancellationToken);
    }
}
