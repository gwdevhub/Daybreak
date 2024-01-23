using Daybreak.Models.GWCA;
using Microsoft.Extensions.Logging;
using System;
using System.Core.Extensions;
using System.Diagnostics;
using System.Extensions;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Daybreak.Services.GWCA;

internal sealed class GWCAClient : IGWCAClient
{
    private const string UrlTemplate = "http://127.0.0.1:[PORT]";
    private const string PortPlaceholder = "[PORT]";
    private const int MinPort = 48557;
    private const int MaxPort = 49000;

    private static readonly (int MinRange, int MaxRange) PortRange = (MinPort, MaxPort);
    private readonly IHttpClient<GWCAClient> httpClient;
    private readonly ILogger<GWCAClient> logger;

    public GWCAClient(
        IHttpClient<GWCAClient> httpClient,
        ILogger<GWCAClient> logger)
    {
        this.httpClient = httpClient.ThrowIfNull();
        this.logger = logger.ThrowIfNull();
    }

    public async Task<bool> CheckAlive(ConnectionContext connectionContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.CheckAlive), connectionContext.Port.ToString());
        try
        {
            var response = await this.httpClient.GetAsync($"{UrlTemplate.Replace(PortPlaceholder, connectionContext.Port.ToString())}/alive", cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                scopedLogger.LogError($"Received non-success status code {response.StatusCode}");
                return false;
            }

            return true;
        }
        catch(Exception e) when (e is TimeoutException or TaskCanceledException)
        {
            scopedLogger.LogError(e, "Failed to get response from server. Connection timed out");
            return false;
        }
    }

    public async Task<ConnectionContext?> Connect(Process process, CancellationToken cancellationToken)
    {
        process.ThrowIfNull();
        var listeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
            .Where(i => i.Port >= PortRange.MinRange && i.Port < PortRange.MaxRange && i.Address.ToString() == IPAddress.Any.ToString());
        foreach(var listener in listeners)
        {
            var scopedLogger = this.logger.CreateScopedLogger(nameof(this.Connect), listener.Port.ToString());
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

                return new ConnectionContext(listener.Port);
            }
            catch (Exception e) when (e is TaskCanceledException or TimeoutException)
            {
                scopedLogger.LogInformation($"Timed out trying to reach port. Continuing");
            }
        }

        return default;
    }

    public async Task<HttpResponseMessage> GetAsync(ConnectionContext connectionContext, string subPath, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger(nameof(this.GetAsync), subPath);
        return await this.httpClient.GetAsync($"{UrlTemplate.Replace(PortPlaceholder, connectionContext.Port.ToString())}/{subPath}");
    }
}
