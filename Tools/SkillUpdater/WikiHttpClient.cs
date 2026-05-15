using System.Net;
using System.Net.Http.Headers;

namespace Daybreak.Tools.SkillUpdater;

/// <summary>
/// Polite HTTP client for the Guild Wars wiki.
/// * Single in-flight request (serialised behind a SemaphoreSlim).
/// * 250 ms minimum spacing between requests.
/// * Exponential back-off on 429/5xx (3 retries: 1s → 2s → 4s).
/// * Custom User-Agent identifying Daybreak.
/// </summary>
public sealed class WikiHttpClient : IDisposable
{
    private static readonly TimeSpan MinSpacing = TimeSpan.FromMilliseconds(250);
    private static readonly TimeSpan[] BackoffDelays =
    [
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(2),
        TimeSpan.FromSeconds(4),
    ];

    private readonly HttpClient httpClient;
    private readonly SemaphoreSlim gate = new(1, 1);
    private DateTimeOffset nextSlot = DateTimeOffset.MinValue;

    public WikiHttpClient(string userAgent)
    {
        this.httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60),
        };
        this.httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetStringAsync(string url, CancellationToken cancellationToken)
    {
        for (var attempt = 0; ; attempt++)
        {
            await this.gate.WaitAsync(cancellationToken);
            try
            {
                var now = DateTimeOffset.UtcNow;
                if (now < this.nextSlot)
                {
                    await Task.Delay(this.nextSlot - now, cancellationToken);
                }

                using var response = await this.httpClient.GetAsync(url, cancellationToken);
                this.nextSlot = DateTimeOffset.UtcNow + MinSpacing;

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync(cancellationToken);
                }

                if (ShouldRetry(response.StatusCode) && attempt < BackoffDelays.Length)
                {
                    var delay = response.Headers.RetryAfter?.Delta ?? BackoffDelays[attempt];
                    Console.Error.WriteLine($"  ! {response.StatusCode}, backing off {delay.TotalSeconds:F0}s (attempt {attempt + 1}/{BackoffDelays.Length})");
                    await Task.Delay(delay, cancellationToken);
                    continue;
                }

                throw new HttpRequestException($"GET {url} → {(int)response.StatusCode} {response.StatusCode}");
            }
            finally
            {
                this.gate.Release();
            }
        }
    }

    private static bool ShouldRetry(HttpStatusCode code) =>
        code == HttpStatusCode.TooManyRequests ||
        (int)code >= 500;

    public void Dispose()
    {
        this.httpClient.Dispose();
        this.gate.Dispose();
    }
}
